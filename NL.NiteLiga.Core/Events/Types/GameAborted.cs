using NL.Core;
using NL.NiteLiga.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Events.Types
{
    public class GameAborted : GameEvent
    {
        public GameAborted(DateTime date)
        {
            this.AddDate = date;
            this.Type = GameEventType.GameAborted;
        }
    }
}
