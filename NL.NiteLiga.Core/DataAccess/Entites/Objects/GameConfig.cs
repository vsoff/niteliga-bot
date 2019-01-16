using Newtonsoft.Json;
using NL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.DataAccess.Entites.Objects
{
    public class GameConfig
    {
        public string Address { get; set; }
        public DateTime GameDate { get; set; }
        public string Description { get; set; }
        public GameTask[] Tasks { get; set; }
        public int[][] TaskGridIndexes { get; set; }

        [JsonIgnore]
        public GameTask[][] TaskGrid { get; set; }
    }
}
