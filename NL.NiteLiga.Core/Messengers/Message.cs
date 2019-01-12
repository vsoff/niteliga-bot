using NL.NiteLiga.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Messengers
{
    public class Message
    {
        public long GameId { get; set; }

        public Player Player { get; set; }
        public Team Team { get; set; }

        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
