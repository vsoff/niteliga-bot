using NL.Core;
using NL.NiteLiga.Core.DataAccess.Entites.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game
{
    public class NiteLigaGameConfiguration : BaseGameConfiguration
    {
        public long GameMatchId { get; set; }
        public GameConfig Config { get; set; }
        public GameSettings Settings { get; set; }
    }
}
