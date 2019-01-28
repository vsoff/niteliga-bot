using Newtonsoft.Json;
using NL.NiteLiga.Core.DataAccess.Entites;
using NL.NiteLiga.Core.DataAccess.Entites.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameControlCenter.NiteLiga.Web.Models
{
    /// <summary>
    /// Веб модель шаблона игры.
    /// </summary>
    public class GameTemplateWebModel
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public GameConfig Config { get; set; }
        public GameSettings Settings { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public static class GameTemplateWebExtension
    {
        /// <summary>
        /// Переводит core объект шаблона игры в веб модель.
        /// </summary>
        public static GameTemplateWebModel ToWebModel(this GameTemplate template)
        {
            return new GameTemplateWebModel
            {
                Id = template.Id,
                Caption = template.Caption,
                Config = JsonConvert.DeserializeObject<GameConfig>(template.JsonConfig),
                Settings = JsonConvert.DeserializeObject<GameSettings>(template.JsonSettings),
                CreateDate = template.CreateDate,
                UpdateDate = template.UpdateDate
            };
        }

        /// <summary>
        /// Переводит веб модель шаблона игры в core объект.
        /// </summary>
        public static GameTemplate ToModel(this GameTemplateWebModel webTemplate)
        {
            return new GameTemplate
            {
                Id = webTemplate.Id,
                Caption = webTemplate.Caption,
                JsonConfig = JsonConvert.SerializeObject(webTemplate.Config),
                JsonSettings = JsonConvert.SerializeObject(webTemplate.Settings),
                CreateDate = webTemplate.CreateDate,
                UpdateDate = webTemplate.UpdateDate
            };
        }
    }
}
