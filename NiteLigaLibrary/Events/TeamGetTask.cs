using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class TeamGetTask : GameEvent
    {
        public int TeamId { get; set; }
        public int DropTaskId { get; set; }

        public TeamGetTask(DateTime date, int teamId, int dropTaskId)
        {
            this.AddDate = date;
            this.Type = GameEventType.TeamGetTask;
            this.TeamId = teamId;
            this.DropTaskId = dropTaskId;
        }

        public override void Run(GameManager gm)
        {

        }
    }
}
