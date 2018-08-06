using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class GameSetting
    {
        public int GameClosingDurationMin { get; set; }
        public int GameDurationMin { get; set; }

        public int Hint1DelaySec { get; set; }
        public int Hint2DelaySec { get; set; }
        public int TaskDropDelaySec { get; set; }
        public int SecondsDelayStart { get; set; }
        public List<int> TeamIds { get; set; }

        public GameSetting()
        {
            // Тут происходит подгрузка настроек из БД
        }
    }
}
