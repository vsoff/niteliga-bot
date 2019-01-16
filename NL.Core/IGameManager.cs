using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.Core
{
    /// <summary>
    /// Менеджер управления игрой.
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Время запуска игры.
        /// </summary>
        DateTime? LaunchTime { get; set; }
        /// <summary>
        /// Время завершения игры.
        /// </summary>
        DateTime? EndTime { get; set; }
        /// <summary>
        /// Статус игры.
        /// </summary>
        GameStatusType Status { get; }

        void Start(BaseGameConfiguration configuration, BaseGameBackup backup = null);
        void Iterate();
        void Abort();
        void Stop();
        BaseGameBackup GetBackup();
    }
}
