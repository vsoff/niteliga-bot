using NL.NiteLiga.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Events
{
    /// <summary>
    /// Событие в игре NiteLiga.
    /// </summary>
    public abstract class GameEvent
    {
        /// <summary>
        /// Дата и время события.
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// Тип события.
        /// </summary>
        public GameEventType Type { get; set; }
    }
}
