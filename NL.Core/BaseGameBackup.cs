using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.Core
{
    public class BaseGameBackup
    {
        public GameStatusType GameStatus { get; set; }
        public DateTime? LaunchTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
