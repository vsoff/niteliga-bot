using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Events.Types
{
    public class GameStopped : GameEvent
    {
        public GameStopped(DateTime date)
        {
            this.AddDate = date;
            this.Type = GameEventType.GameStopped;
        }
    }
}
