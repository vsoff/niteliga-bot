using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.Core
{
    /// <summary>
    /// Состояние игры.
    /// </summary>
    public enum GameStatusType
    {
        /// <summary>
        /// Игра создана, но не запущена.
        /// </summary>
        Created = 0,
        /// <summary>
        /// Игра запущена.
        /// </summary>
        InProgress = 1,
        /// <summary>
        /// Игра в процессе остановки.
        /// </summary>
        Stopped = 2,
        /// <summary>
        /// Игра прервана организатором.
        /// </summary>
        Aborted = 3,
        /// <summary>
        /// Игра завершена.
        /// </summary>
        Ended = 4
    }
}
