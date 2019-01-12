using NL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game
{
    public class GameConfig : BaseGameConfig
    {
        public string Address { get; set; }
        public DateTime GameDate { get; set; }
        public string Description { get; set; }
        public GameTask[] Tasks { get; set; }
        public int[,] TaskGrid { get; set; }
    }
}
