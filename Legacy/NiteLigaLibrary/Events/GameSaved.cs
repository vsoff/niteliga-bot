using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class GameSaved : GameEvent
    {
        public GameSaved(DateTime date)
        {
            this.AddDate = date;
            this.Type = GameEventType.GameSaved;
        }

        public override void Run(GameManager gm)
        {

        }
    }
}
