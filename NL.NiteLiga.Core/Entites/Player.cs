using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NL.NiteLiga.Core.Entites
{
    public class Player
    {
        public long Id { get; set; }
        public long VkId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo50 { get; set; }
        public string Photo200 { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}