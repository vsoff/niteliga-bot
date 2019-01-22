using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    /// <summary>
    /// Текущий прогресс игры команды.
    /// </summary>
    public class TeamGameProgress
    {
        /// <summary>
        /// Время последнего получения подсказки.
        /// </summary>
        public DateTime LastHintTime { get; set; }
        /// <summary>
        /// Время выполнения последнего задания.
        /// </summary>
        public DateTime LastTaskCompleteTime { get; set; }
        /// <summary>
        /// Список заданий команды.
        /// </summary>
        public List<GameTask> Tasks { get; set; }
        /// <summary>
        /// Номер текущего задания.
        /// </summary>
        public int CurrentTaskIndex { get; set; }

        public TeamGameProgress(List<GameTask> tasks, DateTime lastHintTime, DateTime lastTaskCompleteTime, int currentTaskIndex)
        {
            this.Tasks = tasks;
            this.LastTaskCompleteTime = lastTaskCompleteTime;
            this.LastHintTime = lastHintTime;
            this.CurrentTaskIndex = currentTaskIndex;
        }

        public TeamGameProgress(List<GameTask> tasks, DateTime startTime)
        {
            this.Tasks = tasks;
            this.LastTaskCompleteTime = startTime;
            this.LastHintTime = startTime;
            this.CurrentTaskIndex = 0;
        }

        /// <summary>
        /// Возвращает True, если все задания выполнены.
        /// </summary>
        public bool IsAllTaskCompleted()
        {
            return CurrentTaskIndex >= Tasks.Count;
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
            if (IsAllTaskCompleted())
                return null;
            else
                return Tasks[CurrentTaskIndex];
        }
    }
}
