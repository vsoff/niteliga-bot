using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game.Events.Types
{
    public class TeamGetAddress : GameEvent
    {
        public long TeamId { get; set; }
        public int TaskIndex { get; set; }

        public TeamGetAddress(DateTime date, long teamId, int taskIndex)
        {
            this.AddDate = date;
            this.Type = GameEventType.TeamGetAddress;
            this.TeamId = teamId;
            this.TaskIndex = taskIndex;
        }
    }
}
