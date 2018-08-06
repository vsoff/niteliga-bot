﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class GameStopped : GameEvent
    {
        public GameStopped(DateTime date)
        {
            this.AddDate = date;
            this.Type = GameEventType.GameStopped;
        }

        public override void Run(GameManager gm)
        {

        }
    }
}
