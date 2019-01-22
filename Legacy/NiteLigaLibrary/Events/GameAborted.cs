using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class GameAborted : GameEvent
    {
        public GameAborted(DateTime date)
        {
            this.AddDate = date;
            this.Type = GameEventType.GameAborted;
        }

        public override void Run(GameManager gm)
        {
            if (gm.GameStatus == Classes.GameStatusType.InProgress)
            {
                gm.GameStatus = Classes.GameStatusType.Aborted;
                gm.SendBroadcastMessage("Игра прервана организаторами!");
            }
        }
    }
}
