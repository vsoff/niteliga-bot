using NiteLigaLibrary.Classes;
using NiteLigaLibrary.Events;
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
        private DateTime? LaunchTime { get; set; }
        private bool IsGameStarted { get; set; }
        private bool IsGameStopped { get; set; }

        public List<LocalTeam> Teams { get; private set; }
        private GameSetting Setting { get; }
        private GameConfig Config { get; }
        private List<GameEvent> NewEvents { get; }
        public List<GameEvent> StoredEvents { get; }

        public MessageManager Noticer { get; }

        public GameManager(GameConfig config, GameSetting setting)
        {
            this.Setting = setting;
            this.Config = config;
            this.LaunchTime = null;
            this.IsGameStarted = false;
            this.IsGameStopped = false;
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
            if (IsGameStarted)
                throw new Exception("Нельзя начать игру, так как она уже была запущена.");

            IsGameStarted = true;
            LaunchTime = DateTime.Now;

            // Заполняем начальные данные прогресса игры
            for (int i = 0; i < Teams.Count; i++)
                Teams[i].Progress = new TeamGameProgress(Config.Grid[i],
                    ((DateTime)LaunchTime).AddSeconds(Setting.SecondsDelayStart * i));

            NewEvents.Add(new GameStarted((DateTime)LaunchTime));
            SendBroadcastMessage("Игра началась!");
        }

        /// <summary>
        /// Обрабатывает один фрейм игры, на текущий момент времени.
        /// </summary>
        public void Iterate()
        {
            if (!IsGameStarted || IsGameStopped) return;

            // Запоминаем время начала итерации
            DateTime curTime = DateTime.Now;

            int eventsCount = NewEvents.Count;

            // STEP 1: Обрабатываем все ивенты (если они есть)
            if (eventsCount > 0) {
                // Получаем список событий, обрабатываемый на текущей итерации
                List<GameEvent> processedEvents = NewEvents.GetRange(0, eventsCount).OrderBy(x => x.AddDate).ToList();
                NewEvents.RemoveRange(0, eventsCount);

                foreach (GameEvent currentEvent in processedEvents)
                {
                    if (currentEvent.Type == GameEventType.GameStopped)
                        return;
                    currentEvent.Run(this);
                }

                // Добавляем все обработанные ивенты в список
                StoredEvents.AddRange(processedEvents);
            }

            // STEP 2: Добавляем новые ивенты, если прошло необходимое время (подсказка, слив адреса, слив задания)
            foreach (LocalTeam t in Teams)
            {
                if (t.Progress.IsAllTaskCompleted())
                    continue;

                // Кол-во секунд, прошедшее со сдачи последнего задания
                double lastCompleteTaskSec = curTime.Subtract(t.Progress.LastTaskCompleteTime).TotalSeconds;
                // Кол-во секунд, прошедшее с получения последней подсказки
                double lastHintSec = curTime.Subtract(t.Progress.LastHintTime).TotalSeconds;
                
                // Получение первого задания
                if (!t.Progress.IsTeamStarted())
                {
                    // if (curTime > ((DateTime)LaunchTime).AddSeconds(lastCompleteTaskSec * Teams.IndexOf(t)))
                    // NewEvents.Add(new TeamGetTask(curTime, t.Id, 0));
                    throw new NotImplementedException();
                    continue;
                }

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

                if (playerTeam.Progress.GetCurrentTask() == null)
                {
                    Noticer.AddOutputMessage(new Message(m.Player, $"Ваша команда уже выполнила все задания."));
                    continue;
                }

                switch (m.Text)
                {
                    case "задание": Noticer.AddOutputMessage(new Message(m.Player, $"Какая-то подсказка.")); continue;
                }

                if (playerTeam.Progress.GetCurrentTask().Code == m.Text)
                    NewEvents.Add(new PlayerCompletedTask(curTime, playerTeam.Id, m.Player.Id, playerTeam.Progress.CurrentTaskIndex));
                else
                    NewEvents.Add(new PlayerFailedTask(curTime, playerTeam.Id, m.Player.Id, playerTeam.Progress.CurrentTaskIndex));
            }

            // STEP 4: Если прошло достаточно времени - сохраняем игру
            // TODO: ...

            // STEP 5: Если прошло достаточно времени - завершаем игру
            // TODO: ...
        }

        public void Abort()
        {
            if (!IsGameStarted)
                throw new Exception("Нельзя прервать игру, так как она еще не была запущена.");

            NewEvents.Add(new GameAborted(DateTime.Now));
            SendBroadcastMessage("Игра прервана организаторами!");
        }

        public void Stop()
        {
            if (!IsGameStarted)
                throw new Exception("Нельзя остановить игру, так как она еще не была запущена.");
            
            NewEvents.Add(new GameStopped(DateTime.Now));
            SendBroadcastMessage("Игра закончилась!");
        }

        public void SendBroadcastMessage(string message)
        {
            NewEvents.Add(new AdminSendMessage(DateTime.Now, message));

            // Формирование списка всех игроков
            List<LocalPlayer> allPlayers = new List<LocalPlayer>();
            foreach (var team in Teams)
                allPlayers.AddRange(team.Players);

            Noticer.AddOutputMessages(allPlayers, message);
        }

        public void Save()
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
