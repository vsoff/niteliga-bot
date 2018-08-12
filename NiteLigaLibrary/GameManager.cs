using NiteLigaLibrary.Classes;
using NiteLigaLibrary.Events;
using NiteLigaLibrary.Database;
using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary
{
    public class GameManager
    {
        private List<GameEvent> NewEvents { get; }
        public List<GameEvent> StoredEvents { get; }

        public DateTime? LaunchTime { get; set; }
        public DateTime? EndTime { get; set; }
        public GameStatusType GameStatus { get; set; }
        public List<LocalTeam> Teams { get; private set; }

        public GameSetting Setting { get; }
        public GameConfig Config { get; }

        public MessageManager Noticer { get; }

        public GameManager(GameConfig config, GameSetting setting)
        {
            this.Setting = setting;
            this.Config = config;
            this.LaunchTime = null;
            this.GameStatus = GameStatusType.Created;
            this.Noticer = new MessageManager();
            this.NewEvents = new List<GameEvent>();
            this.StoredEvents = new List<GameEvent>();

            // Формирование локальной копии всех команд и игроков участников
            Teams = new List<LocalTeam>();
            using (var db = new NiteLigaContext())
                for (int i = 0; i < Setting.TeamIds.Count; i++)
                    Teams.Add(new LocalTeam(db, Setting.TeamIds[i]));
        }

        public void Start()
        {
            if (GameStatus != GameStatusType.Created)
                throw new Exception("Нельзя начать игру, так как она уже была запущена.");

            GameStatus = GameStatusType.InProgress;
            LaunchTime = DateTime.Now;

            NewEvents.Add(new GameStarted((DateTime)LaunchTime));
            SendBroadcastMessage("Игра началась!");
        }

        /// <summary>
        /// Обрабатывает один фрейм игры, на текущий момент времени.
        /// </summary>
        public void Iterate()
        {
            if (GameStatus != GameStatusType.InProgress && GameStatus != GameStatusType.Stopped)
                return;

            // Запоминаем время начала итерации
            DateTime curTime = DateTime.Now;

            int eventsCount = NewEvents.Count;

            // STEP 1: Обрабатываем все ивенты (если они есть)
            if (eventsCount > 0)
            {
                // Получаем список ивентов, обрабатываемый на текущей итерации
                List<GameEvent> processedEvents = NewEvents.GetRange(0, eventsCount).OrderBy(x => x.AddDate).ToList();
                NewEvents.RemoveRange(0, eventsCount);

                // Выполняем каждый ивент
                foreach (GameEvent currentEvent in processedEvents)
                    currentEvent.Run(this);

                // Добавляем все обработанные ивенты в список
                StoredEvents.AddRange(processedEvents);
            }

            // STEP 2: Добавляем новые ивенты, если прошло необходимое время (подсказка, слив адреса, слив задания)
            if (GameStatus != GameStatusType.Stopped)
                foreach (LocalTeam t in Teams)
                {
                    // Получение первого задания, если прошло достаточно времени и команда еще не начала играть.
                    if (t.Progress == null)
                    {
                        if (curTime > ((DateTime)LaunchTime).AddSeconds(Setting.SecondsDelayStart * Teams.IndexOf(t)))
                            NewEvents.Add(new TeamStartsPlay(curTime, t.Id));
                        continue;
                    }

                    if (t.Progress.IsAllTaskCompleted())
                        continue;

                    // Кол-во секунд, прошедшее со сдачи последнего задания
                    double lastCompleteTaskSec = curTime.Subtract(t.Progress.LastTaskCompleteTime).TotalSeconds;
                    // Кол-во секунд, прошедшее с получения последней подсказки
                    double lastHintSec = curTime.Subtract(t.Progress.LastHintTime).TotalSeconds;

                    // ... сливаем задание
                    if (lastCompleteTaskSec > (Setting.TaskDropDelaySec + Setting.Hint2DelaySec + Setting.Hint1DelaySec))
                        NewEvents.Add(new TeamDropTask(curTime, t.Id, t.Progress.CurrentTaskIndex));
                    // ... сливаем адрес
                    else if (lastCompleteTaskSec > (Setting.Hint2DelaySec + Setting.Hint1DelaySec) && lastHintSec > Setting.Hint2DelaySec)
                        NewEvents.Add(new TeamGetAddress(curTime, t.Id, t.Progress.CurrentTaskIndex));
                    // ... сливаем подсказку
                    else if (lastCompleteTaskSec > (Setting.Hint1DelaySec) && lastHintSec > Setting.Hint1DelaySec)
                        NewEvents.Add(new TeamGetHint(curTime, t.Id, t.Progress.CurrentTaskIndex));
                }

            // STEP 3: Анализируем пришедшие сообщения от пользователей и добавляем новые ивенты
            List<Message> inputMessages = Noticer.PullInput();
            foreach (Message m in inputMessages)
            {
                LocalTeam playerTeam = Teams.First(x => x.Id == m.Player.TeamId);

                if (playerTeam.Progress == null)
                {
                    m.Player.SendMessage(Noticer, $"Ваша команда еще не получила первое задание.");
                    continue;
                }

                if (playerTeam.Progress.GetCurrentTask() == null)
                {
                    m.Player.SendMessage(Noticer, $"Ваша команда уже выполнила все задания.");
                    continue;
                }

                switch (m.Text)
                {
                    case "задание": m.Player.SendMessage(Noticer, $"Какая-то подсказка."); continue;
                }

                if (playerTeam.Progress.GetCurrentTask().Code == m.Text)
                    NewEvents.Add(new PlayerCompletedTask(curTime, playerTeam.Id, m.Player.Id, playerTeam.Progress.CurrentTaskIndex));
                else
                    NewEvents.Add(new PlayerFailedTask(curTime, playerTeam.Id, m.Player.Id, playerTeam.Progress.CurrentTaskIndex));
            }

            // STEP 4: Если прошло достаточно времени - сохраняем игру
            // TODO: ... 

            // STEP 5: Завершение игры
            // Переводим игру в статус "останавливается", если пришло время конца игры.
            if (GameStatus == GameStatusType.InProgress &&
                curTime.Subtract((DateTime)LaunchTime).TotalMinutes > Setting.GameDurationMin)
                NewEvents.Add(new GameStopped(DateTime.Now));

            // Если после остановки игры прошло необходимое кол-во минут - завершаем игру.
            if (GameStatus == GameStatusType.Stopped &&
                EndTime != null && curTime > EndTime?.AddMinutes(Setting.GameClosingDurationMin))
            {
                GameStatus = GameStatusType.Ended;
                SendBroadcastMessage("Приём кодов завершён.");
            }
        }

        public void Abort()
        {
            if (GameStatus != GameStatusType.InProgress)
                throw new Exception("Прервать игру можно только в процессе игры.");

            NewEvents.Add(new GameAborted(DateTime.Now));
        }

        public void Stop()
        {
            if (GameStatus != GameStatusType.InProgress)
                throw new Exception("Остановить игру можно только в процессе игры.");

            NewEvents.Add(new GameStopped(DateTime.Now));
        }

        /// <summary>
        /// Отправляет широковещательное сообщение каждому игроку в игре.
        /// </summary>
        public void SendBroadcastMessage(string message)
        {
            NewEvents.Add(new AdminSendMessage(DateTime.Now, message));

            // Формирование списка всех игроков
            List<LocalPlayer> allPlayers = new List<LocalPlayer>();
            foreach (var team in Teams)
                allPlayers.AddRange(team.Players);

            Noticer.AddOutputMessages(allPlayers, message);
        }

        public object GetLastBackup()
        {
            // TODO: Возвращает актуальный экземпляр бекапа игры.
            throw new NotImplementedException();
        }

        private void Save()
        {
            // TODO: Добавление события "игра сохранена".
            // TODO: Полное сохранение игры на данный момент.
            throw new NotImplementedException();
        }

        public void Restore()
        {
            // TODO: Добавление события "игра восстановлена".
            // TODO: Полное восстановление игры с критической точки.
            throw new NotImplementedException();
        }
    }
}
