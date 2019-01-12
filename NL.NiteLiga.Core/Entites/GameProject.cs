using Newtonsoft.Json;
using NL.NiteLiga.Core.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NL.NiteLiga.Core.Entites
{
    public class GameProject
    {
        public long Id { get; set; }
        public string Config { get; set; }
        public string Caption { get; set; }
        public string Setting { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public GameConfig GetConfig()
        {
            return JsonConvert.DeserializeObject<GameConfig>(this.Config);
        }

        public GameSettings GetSetting()
        {
            return JsonConvert.DeserializeObject<GameSettings>(this.Setting);
        }
    }
}
