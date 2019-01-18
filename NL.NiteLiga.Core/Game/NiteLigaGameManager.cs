using NL.Core;
using NL.NiteLiga.Core.DataAccess.Entites;
using NL.NiteLiga.Core.DataAccess.Entites.Objects;
using NL.NiteLiga.Core.DataAccess.Repositories;
using NL.NiteLiga.Core.Game;
using NL.NiteLiga.Core.Game.Events;
using NL.NiteLiga.Core.Game.Events.Types;
using NL.NiteLiga.Core.Game.Messengers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NL.NiteLiga.Core.Game
{
    /// <summary>
    /// Менеджер управления игрой `NiteLiga`.
    /// </summary>
    public class NiteLigaGameManager : IGameManager
    {
        /// <inheritdoc/>
        public GameStatusType Status { get; private set; }
        /// <inheritdoc/>
        public DateTime? LaunchTime { get; set; }
        /// <inheritdoc/>
        public DateTime? EndTime { get; set; }

        private readonly object _locker;
        private bool _isIterateInProgress;

        private NiteLigaGameConfiguration _configuration;
        private List<GameEvent> _newEvents;
        private List<GameEvent> _storedEvents;
        private GameTeamContainer[] _teamsContainers;

        private readonly IMessenger _messenger;
        private readonly IMessagePool _messagePool;
        private readonly ITeamsRepository _teamsRepository;

        public NiteLigaGameManager(
            IMessenger messenger,
            IMessagePool messagePool,
            ITeamsRepository teamsRepository)
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(_messenger));
            _messagePool = messagePool ?? throw new ArgumentNullException(nameof(_messagePool));
            _teamsRepository = teamsRepository ?? throw new ArgumentNullException(nameof(_teamsRepository));

            _locker = new object();
            Status = GameStatusType.Created;
        }

        public void Start(BaseGameConfiguration configuration, BaseGameBackup backup = null)
        {
            NiteLigaGameConfiguration niteLigaConfiguration = configuration as NiteLigaGameConfiguration
                ?? throw new ArgumentException(nameof(configuration));

            if (Status != GameStatusType.Created)
                throw new Exception("Нельзя начать игру, так как она уже была запущена.");

            // Предстартовые настройки.
            if (backup != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                LaunchTime = DateTime.Now;
                Status = GameStatusType.InProgress;

                _configuration = niteLigaConfiguration;
                _newEvents = new List<GameEvent> { new GameStarted((DateTime)LaunchTime) };
                _storedEvents = new List<GameEvent>();

                // Собираем всю необходимую информацию о командах в контейнеры.
                List<GameTeamContainer> teamContainerList = new List<GameTeamContainer>();
                Team[] teams = _teamsRepository.GetTeams(_configuration.Settings.TeamIds);
                for (int i = 0; i < _configuration.Settings.TeamIds.Length; i++)
                {
                    long teamId = _configuration.Settings.TeamIds[i];
                    Team team = teams.First(x => x.Id == teamId);
                    teamContainerList.Add(new GameTeamContainer
                    {
                        Order = i,
                        Team = team,
                        Progress = new GameTeamProgress(_configuration.Config.TaskGrid[i])
                    });
                }
                _teamsContainers = teamContainerList.ToArray();
            }

            _messagePool.NewQueue(_configuration.GameMatchId);
        }

        public BaseGameBackup GetBackup()
        {
            throw new NotImplementedException();
        }

        public void Abort()
        {
            if (Status != GameStatusType.InProgress)
                throw new Exception("Прервать игру можно только в процессе игры.");

            _newEvents.Add(new GameAborted(DateTime.Now));
        }

        public void Stop()
        {
            if (Status != GameStatusType.InProgress)
                throw new Exception("Остановить игру можно только в процессе игры.");

            _newEvents.Add(new GameStopped(DateTime.Now));
        }

        public void Iterate()
        {
            if (Status != GameStatusType.InProgress && Status != GameStatusType.Stopped)
                return;

            lock (_locker)
            {
                if (_isIterateInProgress)
                    throw new Exception("Итерация рассчёта фрейма игры уже производится");
                _isIterateInProgress = true;
            }

            CalculateFrame();

            _isIterateInProgress = false;
        }

        public void Dispose()
        {
            if (Status == GameStatusType.InProgress)
                Stop();

            _messagePool.DeleteQueue(_configuration.GameMatchId);
        }

        #region Логика игры

        private void CalculateFrame()
        {
            // Запоминаем время начала итерации
            DateTime curTime = DateTime.Now;

            // STEP 1: Обрабатываем все ивенты (если они есть)
            if (_newEvents.Count > 0)
                HandleEvents();

            // STEP 2: Добавляем новые ивенты, если прошло необходимое время (подсказка, слив адреса, слив задания)
            // [Когда игра останавливается новые задания уже не выдаются]
            if (Status != GameStatusType.Stopped)
                AddNewEvents(curTime);

            // STEP 3: Анализируем пришедшие сообщения от пользователей и добавляем новые ивенты
            HandleMessages(curTime);

            // STEP 4: Завершение игры
            // Переводим игру в статус "останавливается", если пришло время конца игры.
            if (Status == GameStatusType.InProgress &&
                curTime.Subtract((DateTime)LaunchTime).TotalMinutes > _configuration.Settings.GameDurationMin)
                _newEvents.Add(new GameStopped(DateTime.Now));

            // Если после остановки игры прошло необходимое кол-во минут - завершаем игру.
            if (Status == GameStatusType.Stopped &&
                EndTime != null && curTime > EndTime?.AddMinutes(_configuration.Settings.GameClosingDurationMin))
            {
                Status = GameStatusType.Ended;
                SendBroadcastMessage("Приём кодов завершён.");
            }
        }

        /// <summary>
        /// Обрабатывает все новые события игре.
        /// </summary>
        private void HandleEvents()
        {
            int eventsCount = _newEvents.Count;

            // Получаем список ивентов, обрабатываемый на текущей итерации
            List<GameEvent> processedEvents = _newEvents.GetRange(0, eventsCount).OrderBy(x => x.AddDate).ToList();
            _newEvents.RemoveRange(0, eventsCount);

            // Выполняем каждый ивент
            foreach (GameEvent currentEvent in processedEvents)
                HandleEvent(currentEvent);

            // Добавляем все обработанные ивенты в список
            _storedEvents.AddRange(processedEvents);
        }

        /// <summary>
        /// Добавляет новые события с учётом времени.
        /// </summary>
        private void AddNewEvents(DateTime curTime)
        {
            foreach (GameTeamContainer container in _teamsContainers)
            {
                // Получение первого задания, если прошло достаточно времени и команда еще не начала играть.
                if (!container.Progress.IsTeamStartsPlay())
                {
                    if (curTime > ((DateTime)LaunchTime).AddSeconds(_configuration.Settings.SecondsDelayStart * container.Order))
                        _newEvents.Add(new TeamStartsPlay(curTime, container.Team.Id));
                    continue;
                }

                if (container.Progress.IsAllTaskCompleted())
                    continue;

                // Кол-во секунд, прошедшее со сдачи последнего задания
                double lastCompleteTaskSec = curTime.Subtract(container.Progress.LastTaskCompleteTime.Value).TotalSeconds;
                // Кол-во секунд, прошедшее с получения последней подсказки
                double lastHintSec = curTime.Subtract(container.Progress.LastHintTime.Value).TotalSeconds;

                // ... сливаем задание
                if (lastCompleteTaskSec > (_configuration.Settings.TaskDropDelaySec + _configuration.Settings.Hint2DelaySec + _configuration.Settings.Hint1DelaySec))
                    _newEvents.Add(new TeamDropTask(curTime, container.Team.Id, container.Progress.CurrentTaskIndex));
                // ... сливаем адрес
                else if (lastCompleteTaskSec > (_configuration.Settings.Hint2DelaySec + _configuration.Settings.Hint1DelaySec) && lastHintSec > _configuration.Settings.Hint2DelaySec)
                    _newEvents.Add(new TeamGetAddress(curTime, container.Team.Id, container.Progress.CurrentTaskIndex));
                // ... сливаем подсказку
                else if (lastCompleteTaskSec > (_configuration.Settings.Hint1DelaySec) && lastHintSec > _configuration.Settings.Hint1DelaySec)
                    _newEvents.Add(new TeamGetHint(curTime, container.Team.Id, container.Progress.CurrentTaskIndex));
            }
        }

        /// <summary>
        /// Обрабатывает пришедшие от игроков сообщения.
        /// </summary>
        private void HandleMessages(DateTime curTime)
        {
            Message[] inputMessages = _messagePool.GetMessages(_configuration.GameMatchId);
            foreach (Message m in inputMessages)
            {
                GameTeamContainer container = _teamsContainers.First(x => x.Team.Id == m.Team.Id);

                if (container.Progress == null)
                {
                    _messenger.SendMessage(m.Player, "Ваша команда еще не получила первое задание.");
                    continue;
                }

                if (container.Progress.GetCurrentTask() == null)
                {
                    _messenger.SendMessage(m.Player, "Ваша команда уже выполнила все задания.");
                    continue;
                }

                // Обрабатываем дополнительные команды.
                switch (m.Text)
                {
                    case "/task":
                    case "/задание": _messenger.SendMessage(m.Player, "Какая-то подсказка."); continue;
                    case "/stat":
                    case "/стат": _messenger.SendMessage(m.Player, "Какая-то статистика."); continue;
                }

                // Сверяем введённый текст и код задания.
                if (container.Progress.GetCurrentTask().Code.ToLower() == m.Text.ToLower())
                    _newEvents.Add(new PlayerCompletedTask(curTime, m.Team.Id, m.Player.Id, container.Progress.CurrentTaskIndex));
                else
                    _newEvents.Add(new PlayerFailedTask(curTime, m.Team.Id, m.Player.Id, container.Progress.CurrentTaskIndex));
            }
        }

        private void SendBroadcastMessage(string message)
        {
            Player[] allPlayers = _teamsContainers.SelectMany(x => x.Team.Players).ToArray();
            _messenger.SendMessage(allPlayers, message);
        }

        /// <summary>
        /// Обрабатывает событие в зависимости его типа.
        /// </summary>
        /// <remarks>Хорошо было бы это перенести в отдельные обработчики EventHandler&ltT&gt</remarks>
        private void HandleEvent(GameEvent gameEvent)
        {
            switch (gameEvent.Type)
            {
                case GameEventType.GameAborted:
                    {
                        if (Status == GameStatusType.InProgress)
                        {
                            Status = GameStatusType.Aborted;
                            SendBroadcastMessage("Игра прервана организаторами!");
                        }
                        break;
                    }
                case GameEventType.GameStopped:
                    {
                        var gevent = gameEvent as GameStopped;

                        if (Status == GameStatusType.InProgress)
                        {
                            Status = GameStatusType.Stopped;
                            EndTime = gevent.AddDate;
                            SendBroadcastMessage("Игра закончена, возвращайтесь на место сбора.");
                        }
                        break;
                    }
                case GameEventType.PlayerCompletedTask:
                    {
                        var gevent = gameEvent as PlayerCompletedTask;
                        var teamContainer = _teamsContainers.First(x => x.Team.Id == gevent.TeamId);
                        teamContainer.Progress.CompleteTask(gevent.AddDate, gevent.TaskIndex);

                        if (teamContainer.Progress.IsAllTaskCompleted())
                            _messenger.SendMessage(teamContainer.Team, "Код правильный!\r\nВсе задания выполнены, возвращайтесь на место сбора.");
                        else
                            _messenger.SendMessage(teamContainer.Team, $"Код правильный!\r\nВаше следующее задание:\r\n{teamContainer.Progress.GetCurrentTask().Task}");
                        break;
                    }
                case GameEventType.PlayerFailedTask:
                    {
                        var gevent = gameEvent as PlayerFailedTask;
                        Player player = _teamsContainers.SelectMany(x => x.Team.Players).First(x => x.Id == gevent.PlayerId);
                        _messenger.SendMessage(player, "Код неправильный!");
                        break;
                    }
                case GameEventType.TeamStartsPlay:
                    {
                        var gevent = gameEvent as TeamStartsPlay;
                        var teamContainer = _teamsContainers.First(x => x.Team.Id == gevent.TeamId);
                        teamContainer.Progress.StartPlay(gevent.AddDate);
                        _messenger.SendMessage(teamContainer.Team, $"Ваше первое задание: {teamContainer.Progress.GetCurrentTask().Task}");
                        break;
                    }
                case GameEventType.TeamGetHint:
                    {
                        var gevent = gameEvent as TeamGetHint;
                        var teamContainer = _teamsContainers.First(x => x.Team.Id == gevent.TeamId);
                        teamContainer.Progress.LastHintTime = gevent.AddDate;
                        _messenger.SendMessage(teamContainer.Team, $"Подсказка: {teamContainer.Progress.GetCurrentTask().Hint1}");
                        break;
                    }
                case GameEventType.TeamGetAddress:
                    {
                        var gevent = gameEvent as TeamGetAddress;
                        var teamContainer = _teamsContainers.First(x => x.Team.Id == gevent.TeamId);
                        teamContainer.Progress.LastHintTime = gevent.AddDate;
                        _messenger.SendMessage(teamContainer.Team, $"Слив адреса: {teamContainer.Progress.GetCurrentTask().Hint2} ({teamContainer.Progress.GetCurrentTask().Address})");
                        break;
                    }
                case GameEventType.TeamDropTask:
                    {
                        var gevent = gameEvent as TeamDropTask;
                        var teamContainer = _teamsContainers.First(x => x.Team.Id == gevent.TeamId);
                        teamContainer.Progress.CompleteTask(gevent.AddDate, gevent.TaskIndex);

                        if (teamContainer.Progress.IsAllTaskCompleted())
                            _messenger.SendMessage(teamContainer.Team, "Задание слито!\r\nВсе задания выполнены, возвращайтесь на место сбора.");
                        else
                            _messenger.SendMessage(teamContainer.Team, $"Задание слито!\r\nВаше следующее задание:\r\n{teamContainer.Progress.GetCurrentTask().Task}");
                        break;
                    }
                case GameEventType.AdminSendMessage:
                    {
                        var gevent = gameEvent as AdminSendMessage;
                        SendBroadcastMessage(gevent.Message);
                        break;
                    }
                case GameEventType.GameStarted: break;
                case GameEventType.GameRestored: break;
                case GameEventType.GameSaved: break;
                default: throw new NotImplementedException();
            }
        }
        #endregion
    }
}
