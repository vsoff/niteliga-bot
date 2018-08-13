using NiteLigaLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class TeamGetAddress : GameEvent
    {
        public int TeamId { get; set; }
        public int TaskIndex { get; set; }

        public TeamGetAddress(DateTime date, int teamId, int taskIndex)
        {
            this.AddDate = date;
            this.Type = GameEventType.TeamGetAddress;
            this.TeamId = teamId;
            this.TaskIndex = taskIndex;
        }

        public override void Run(GameManager gm)
        {
            LocalTeam team = gm.Teams.First(x => x.Id == TeamId);
            team.Progress.LastHintTime = AddDate;
            GameTask task = team.Progress.GetCurrentTask();
            team.SendMessage(gm.Noticer, $"Слив адреса: {task.Hint2} ({task.Address})");
        }
    }
}
