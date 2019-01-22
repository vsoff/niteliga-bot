using NiteLigaLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class TeamDropTask : GameEvent
    {
        public int TeamId { get; set; }
        public int TaskIndex { get; set; }

        public TeamDropTask(DateTime date, int teamId, int taskIndex)
        {
            this.AddDate = date;
            this.Type = GameEventType.TeamDropTask;
            this.TeamId = teamId;
            this.TaskIndex = taskIndex;
        }

        public override void Run(GameManager gm)
        {
            LocalTeam team = gm.Teams.First(x => x.Id == TeamId);
            team.Progress.CompleteTask(AddDate, TaskIndex);

            if (team.Progress.IsAllTaskCompleted())
                team.SendMessage(gm.Noticer, $"Задание слито!\r\nВсе задания выполнены, возвращайтесь на место сбора.");
            else
                team.SendMessage(gm.Noticer, $"Задание слито!\r\nВаше следующее задание:\r\n{team.Progress.GetCurrentTask().Task}");
        }
    }
}
