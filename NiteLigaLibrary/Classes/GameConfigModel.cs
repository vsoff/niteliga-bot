using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class GameConfigModel
    {
        public string Address { get; set; }
        public DateTime GameDate { get; set; }
        public string Description { get; set; }
        public List<GameTask> Tasks { get; set; }
        public List<List<int>> TaskGrid { get; set; }
    }
}
