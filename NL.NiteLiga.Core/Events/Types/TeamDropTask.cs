using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Events.Types
{
    public class TeamDropTask : GameEvent
    {
        public long TeamId { get; set; }
        public int TaskIndex { get; set; }

        public TeamDropTask(DateTime date, long teamId, int taskIndex)
        {
            this.AddDate = date;
            this.Type = GameEventType.TeamDropTask;
            this.TeamId = teamId;
            this.TaskIndex = taskIndex;
        }
    }
}
