using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class PlayerCompletedTask : GameEvent
    {
        public int PlayerId { get; set; }
        public int TaskIndex { get; set; }
        public int TeamId { get; set; }

        public PlayerCompletedTask(DateTime date, int teamId, int playerId, int taskId)
        {
            this.AddDate = date;
            this.PlayerId = playerId;
            this.TaskIndex = taskId;
            this.TeamId = teamId;
            this.Type = GameEventType.PlayerCompletedTask;
        }

        public override void Run(GameManager gm)
        {
            var team = gm.Teams.First(x => x.Id == TeamId);
            team.Progress.CompleteTask(AddDate, TaskIndex);

            if (team.Progress.IsAllTaskCompleted())
                team.SendMessage(gm.Noticer, $"Код правильный!\r\nВсе задания выполнены, возвращайтесь на место сбора.");
            else
                team.SendMessage(gm.Noticer, $"Код правильный!\r\nВаше следующее задание:\r\n{team.Progress.GetCurrentTask().Task}");
        }
    }
}
