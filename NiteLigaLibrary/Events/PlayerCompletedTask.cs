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
            team.SendMessage(gm.Noticer, $"Задание выполнено!\nСледующее задание:\n{team.Progress.GetCurrentTask()?.Task ?? "Нету"}");
        }
    }
}
