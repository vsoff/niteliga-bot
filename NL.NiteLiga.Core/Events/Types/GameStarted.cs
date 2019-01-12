﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Events.Types
{
    public class GameStarted : GameEvent
    {
        public GameStarted(DateTime date)
        {
            this.AddDate = date;
            this.Type = GameEventType.GameStarted;
        }
    }
}
