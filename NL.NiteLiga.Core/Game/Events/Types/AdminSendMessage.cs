using NL.NiteLiga.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game.Events.Types
{
    public class AdminSendMessage : GameEvent
    {
        public string Message { get; }

        public AdminSendMessage(DateTime date, string message)
        {
            this.AddDate = date;
            this.Message = message;
            this.Type = GameEventType.AdminSendMessage;
        }
    }
}
