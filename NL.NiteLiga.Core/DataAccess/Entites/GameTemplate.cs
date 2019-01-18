using Newtonsoft.Json;
using NL.NiteLiga.Core.DataAccess.Entites.Objects;
using NL.NiteLiga.Core.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NL.NiteLiga.Core.DataAccess.Entites
{
    public class GameTemplate
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string JsonConfig { get; set; }
        public string JsonSettings { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        [NotMapped]
        public GameConfig Config { get; set; }
        [NotMapped]
        public GameSettings Settings { get; set; }
    }
}
