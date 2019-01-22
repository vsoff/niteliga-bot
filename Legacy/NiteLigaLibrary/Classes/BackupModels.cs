using NiteLigaLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class GameBackupModel
    {
        public DateTime? EndTime { get; set; }
        public DateTime? LaunchTime { get; set; }
        public GameStatusType GameStatus { get; set; }
        public List<GameEvent> StoredEvents { get; set; }
        public Dictionary<int, TeamProgressBackupModel> TeamProgress { get; set; }
    }

    public class TeamProgressBackupModel
    {
        public DateTime LastHintTime { get; set; }
        public DateTime LastTaskCompleteTime { get; set; }
        public int CurrentTaskIndex { get; set; }
    }
}
