using NiteLigaLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class TeamGetHint : GameEvent
    {
        public int TeamId { get; set; }
        public int DropTaskId { get; set; }

        public TeamGetHint(DateTime date, int teamId, int dropTaskId)
        {
            this.AddDate = date;
            this.Type = GameEventType.TeamGetHint;
            this.TeamId = teamId;
            this.DropTaskId = dropTaskId;
        }

        public override void Run(GameManager gm)
        {
            LocalTeam team = gm.Teams.First(x => x.Id == TeamId);
            team.Progress.LastHintTime = AddDate;
            GameTask task = team.Progress.GetCurrentTask();
            team.SendMessage(gm.Noticer, $"Подсказка: {task.Hint1}");
        }
    }
}
