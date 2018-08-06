﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NiteLigaLibrary.Database.Models
{
    public class StoredGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Caption { get; set; }
        public string JSON { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
