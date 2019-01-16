using NL.NiteLiga.Core.DataAccess.Entites.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game
{
    /// <summary>
    /// Текущий прогресс команды в игре.
    /// </summary>
    public class GameTeamProgress
    {
        /// <summary>
        /// Перечень заданий команды.
        /// </summary>
        public GameTask[] Tasks { get; set; }
        /// <summary>
        /// Номер текущего задания.
        /// </summary>
        public int CurrentTaskIndex { get; set; }
        /// <summary>
        /// Время последнего получения подсказки.
        /// </summary>
        public DateTime? LastHintTime { get; set; }
        /// <summary>
        /// Время выполнения последнего задания.
        /// </summary>
        public DateTime? LastTaskCompleteTime { get; set; }

        public GameTeamProgress(GameTask[] tasks, DateTime lastHintTime, DateTime lastTaskCompleteTime, int currentTaskIndex)
        {
            this.Tasks = tasks;
            this.LastTaskCompleteTime = lastTaskCompleteTime;
            this.LastHintTime = lastHintTime;
            this.CurrentTaskIndex = currentTaskIndex;
        }

        public GameTeamProgress(GameTask[] tasks)
        {
            this.Tasks = tasks;
            this.LastTaskCompleteTime = null;
            this.LastHintTime = null;
            this.CurrentTaskIndex = -1;
        }

        /// <summary>
        /// Запускает прогресс игры для команды.
        /// </summary>
        public void StartPlay(DateTime startTime)
        {
            this.LastTaskCompleteTime = startTime;
            this.LastHintTime = startTime;
            this.CurrentTaskIndex = 0;
        }

        /// <summary>
        /// Возвращает True, если команда начала играть.
        /// </summary>
        /// <returns></returns>
        public bool IsTeamStartsPlay()
        {
            return CurrentTaskIndex != -1;
        }

        /// <summary>
        /// Возвращает True, если все задания выполнены.
        /// </summary>
        public bool IsAllTaskCompleted()
        {
            return CurrentTaskIndex >= Tasks.Length;
        }

        /// <summary>
        /// Выполняет задание c указанным индексом.
        /// </summary>
        public bool CompleteTask(DateTime completeDate, int completedTaskId)
        {
            if (completedTaskId != CurrentTaskIndex || IsAllTaskCompleted())
                return false;

            CurrentTaskIndex++;
            LastHintTime = completeDate;
            LastTaskCompleteTime = completeDate;

            return true;
        }

        /// <summary>
        /// Получить экземпляр текущего задания.
        /// </summary>
        public GameTask GetCurrentTask()
        {
            if (!IsTeamStartsPlay())
                throw new Exception("Нельзя получить текущее задание, так как команда еще не начала играть");

            if (IsAllTaskCompleted())
                return null;

            return Tasks[CurrentTaskIndex];
        }
    }
}
