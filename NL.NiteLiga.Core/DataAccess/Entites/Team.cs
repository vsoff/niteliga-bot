using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NL.NiteLiga.Core.DataAccess.Entites
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Division { get; set; }
        public DateTime RegistrationDate { get; set; }

        [NotMapped]
        public Player[] Players { get; set; }
    }
}