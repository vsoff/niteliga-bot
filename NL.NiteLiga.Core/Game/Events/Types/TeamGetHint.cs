using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game.Events.Types
{
    public class TeamGetHint : GameEvent
    {
        public long TeamId { get; set; }
        public int TaskIndex { get; set; }

        public TeamGetHint(DateTime date, long teamId, int taskIndex)
        {
            this.AddDate = date;
            this.Type = GameEventType.TeamGetHint;
            this.TeamId = teamId;
            this.TaskIndex = taskIndex;
        }
    }
}
