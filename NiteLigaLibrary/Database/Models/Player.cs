using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NiteLigaLibrary.Database.Models
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long VkId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo50 { get; set; }
        public string Photo200 { get; set; }

        public virtual ICollection<PlayerInTeam> PlayersInTeams { get; set; }
    }
}