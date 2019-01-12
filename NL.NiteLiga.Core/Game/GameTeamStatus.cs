using NL.NiteLiga.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game
{
    class GameTeamContainer
    {
        /// <summary>
        /// Порядок старта команды.
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Команда.
        /// </summary>
        public Team Team { get; set; }
        /// <summary>
        /// Прогресс команды в игре.
        /// </summary>
        public GameTeamProgress Progress { get; set; }
    }
}
