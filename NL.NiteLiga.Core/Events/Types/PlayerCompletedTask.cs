using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Events.Types
{
    public class PlayerCompletedTask : GameEvent
    {
        public long PlayerId { get; set; }
        public int TaskIndex { get; set; }
        public long TeamId { get; set; }

        public PlayerCompletedTask(DateTime date, long teamId, long playerId, int taskId)
        {
            this.AddDate = date;
            this.PlayerId = playerId;
            this.TaskIndex = taskId;
            this.TeamId = teamId;
            this.Type = GameEventType.PlayerCompletedTask;
        }
    }
}
