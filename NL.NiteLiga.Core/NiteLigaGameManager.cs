using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NL.Core;
using NL.NiteLiga.Core.Events;
using NL.NiteLiga.Core.Events.Types;
using NL.NiteLiga.Core.Game;
using NL.NiteLiga.Core.Messengers;

namespace NL.NiteLiga.Core
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
        /// <inheritdoc/>
        public long GameId { get; set; }

        private readonly object _locker;
        private bool _isIterateInProgress;

        private GameConfig _config;
        private GameSettings _settings;
        private List<GameEvent> _newEvents;
        private List<GameEvent> _storedEvents;
        private GameTeamContainer[] _teamsContainers;

        private readonly IMessager _messager;
        private readonly IMessengePool _messagePool;

        public NiteLigaGameManager()
        {
            Status = GameStatusType.Created;
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

        public void Start(BaseGameSettings settings, BaseGameConfig config, BaseGameBackup backup = null)
        {
            GameSettings niteLigaSetting = settings as GameSettings;
            GameConfig niteLigaConfig = config as GameConfig;

            if (niteLigaSetting == null)
                throw new ArgumentNullException();
            if (niteLigaConfig == null)
                throw new ArgumentNullException();

            if (Status != GameStatusType.Created)
                throw new Exception("Нельзя начать игру, так как она уже была запущена.");

            Status = GameStatusType.InProgress;
            LaunchTime = DateTime.Now;

            _newEvents.Add(new GameStarted((DateTime)LaunchTime));
        }

        private void SendBroadcastMessage(string message)
        {
            throw new NotImplementedException();
        }

        private void HandleEvent(GameEvent gameEvent)
        {
            switch (gameEvent.Type)
            {
                default: throw new NotImplementedException();
            }
        }

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
                curTime.Subtract((DateTime)LaunchTime).TotalMinutes > _settings.GameDurationMin)
                _newEvents.Add(new GameStopped(DateTime.Now));

            // Если после остановки игры прошло необходимое кол-во минут - завершаем игру.
            if (Status == GameStatusType.Stopped &&
                EndTime != null && curTime > EndTime?.AddMinutes(_settings.GameClosingDurationMin))
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
                if (container.Progress == null)
                {
                    if (curTime > ((DateTime)LaunchTime).AddSeconds(_settings.SecondsDelayStart * container.Order))
                        _newEvents.Add(new TeamStartsPlay(curTime, container.Team.Id));
                    continue;
                }

                if (container.Progress.IsAllTaskCompleted())
                    continue;

                // Кол-во секунд, прошедшее со сдачи последнего задания
                double lastCompleteTaskSec = curTime.Subtract(container.Progress.LastTaskCompleteTime).TotalSeconds;
                // Кол-во секунд, прошедшее с получения последней подсказки
                double lastHintSec = curTime.Subtract(container.Progress.LastHintTime).TotalSeconds;

                // ... сливаем задание
                if (lastCompleteTaskSec > (_settings.TaskDropDelaySec + _settings.Hint2DelaySec + _settings.Hint1DelaySec))
                    _newEvents.Add(new TeamDropTask(curTime, container.Team.Id, container.Progress.CurrentTaskIndex));
                // ... сливаем адрес
                else if (lastCompleteTaskSec > (_settings.Hint2DelaySec + _settings.Hint1DelaySec) && lastHintSec > _settings.Hint2DelaySec)
                    _newEvents.Add(new TeamGetAddress(curTime, container.Team.Id, container.Progress.CurrentTaskIndex));
                // ... сливаем подсказку
                else if (lastCompleteTaskSec > (_settings.Hint1DelaySec) && lastHintSec > _settings.Hint1DelaySec)
                    _newEvents.Add(new TeamGetHint(curTime, container.Team.Id, container.Progress.CurrentTaskIndex));
            }
        }

        /// <summary>
        /// Обрабатывает пришедшие от игроков сообщения.
        /// </summary>
        private void HandleMessages(DateTime curTime)
        {
            Message[] inputMessages = _messagePool.GetMessages(GameId);
            foreach (Message m in inputMessages)
            {
                GameTeamContainer container = _teamsContainers.First(x => x.Team.Id == m.Team.Id);

                if (container.Progress == null)
                {
                    _messager.SendMessage(m.Player, "Ваша команда еще не получила первое задание.");
                    continue;
                }

                if (container.Progress.GetCurrentTask() == null)
                {
                    _messager.SendMessage(m.Player, "Ваша команда уже выполнила все задания.");
                    continue;
                }

                // Обрабатываем дополнительные команды.
                switch (m.Text)
                {
                    case "/задание": _messager.SendMessage(m.Player, "Какая-то подсказка."); continue;
                }

                // Сверяем введённый текст и код задания.
                if (container.Progress.GetCurrentTask().Code.ToLower() == m.Text.ToLower())
                    _newEvents.Add(new PlayerCompletedTask(curTime, m.Team.Id, m.Player.Id, container.Progress.CurrentTaskIndex));
                else
                    _newEvents.Add(new PlayerFailedTask(curTime, m.Team.Id, m.Player.Id, container.Progress.CurrentTaskIndex));
            }
        }
    }
}
