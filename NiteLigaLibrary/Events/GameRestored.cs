using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class GameRestored : GameEvent
    {
        public GameRestored(DateTime date)
        {
            this.AddDate = date;
            this.Type = GameEventType.GameRestored;
        }

        public override void Run(GameManager gm)
        {

        }
    }
}
