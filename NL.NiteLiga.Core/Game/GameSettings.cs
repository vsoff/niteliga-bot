using NL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game
{
    public class GameSettings : BaseGameSettings
    {
        public int GameClosingDurationMin { get; set; }
        public int GameDurationMin { get; set; }

        public int Hint1DelaySec { get; set; }
        public int Hint2DelaySec { get; set; }
        public int TaskDropDelaySec { get; set; }
        public int SecondsDelayStart { get; set; }
        public int[] TeamIds { get; set; }

        public bool Verify(out List<string> errors)
        {
            errors = new List<string>();

            if (GameDurationMin < 1)
                errors.Add("Длительность игры должна быть дольше 1 минуты");

            if (GameClosingDurationMin < 1)
                errors.Add("Длительность завершения игры должна быть больше 1 минуты");

            if (Hint1DelaySec < 15)
                errors.Add("Время до первой подсказки должно быть больше 15 секунд");

            if (Hint2DelaySec < 15)
                errors.Add("Время до слива адреса должно быть больше 15 секунд");

            if (TaskDropDelaySec < 10)
                errors.Add("Время до слива задания должно быть больше 10 секунд");

            if (SecondsDelayStart < 5)
                errors.Add("Время между стартами команд должно быть больше 5 секунд");

            if (TeamIds.Length < 1)
                errors.Add("В игре должна участвовать как минимум одна команда");

            return errors.Count == 0;
        }
    }
}
