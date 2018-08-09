using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Database.Models
{
    public enum PlayerInTeamStatus
    {
        Captain = 0,
        Player = 1,
        Legionary = 2,
        Invited = 3,
        Requested = 4
    }

    public class PlayerInTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }

        [Range(0, 4)]
        public PlayerInTeamStatus Status { get; set; }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
