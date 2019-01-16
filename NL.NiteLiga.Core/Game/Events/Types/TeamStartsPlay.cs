using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game.Events.Types
{
    public class TeamStartsPlay : GameEvent
    {
        public long TeamId { get; set; }

        public TeamStartsPlay(DateTime date, long teamId)
        {
            this.AddDate = date;
            this.Type = GameEventType.TeamStartsPlay;
            this.TeamId = teamId;
        }
    }
}
