using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.DataAccess.Entites
{
    public enum MembershipStatus
    {
        /// <summary>
        /// Запрошено членство в команде.
        /// </summary>
        Requested = 0,
        /// <summary>
        /// Приглашён в команду. 
        /// </summary>
        Invited = 1,
        /// <summary>
        /// Легионер команды.
        /// </summary>
        Legionary = 2,
        /// <summary>
        /// Участник команды.
        /// </summary>
        Player = 3,
        /// <summary>
        /// Капитан команды.
        /// </summary>
        Captain = 4
    }

    public class Membership
    {
        public long Id { get; set; }
        public long TeamId { get; set; }
        public long PlayerId { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public MembershipStatus Status { get; set; }

        public virtual Team Team { get; set; }
        public virtual Player Player { get; set; }
    }
}
