using Newtonsoft.Json;
using NiteLigaLibrary.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NiteLigaLibrary.Database.Models
{
    public class GameProject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Caption { get; set; }
        public string Setting { get; set; }
        public string Config { get; set; }

        public virtual ICollection<GameArchive> GameArchives { get; set; }

        /// <summary>
        /// Проверяет правильность данных содержащихся в полях Caption и Config
        /// </summary>
        public bool VerifyData(out List<string> errors)
        {
            List<string> errs;
            GameSetting gs = null;
            GameConfigModel gcm = null;

            errors = new List<string>();

            try
            {
                gs = JsonConvert.DeserializeObject<GameSetting>(Setting);
                gs.Verify(out errs);
                errors.AddRange(errs);
            }
            catch
            {
                errors.Add("Setting не соответствует модели");
            }

            try
            {
                gcm = JsonConvert.DeserializeObject<GameConfigModel>(Config);
                gcm.Verify(out errs);
                errors.AddRange(errs);
            }
            catch
            {
                errors.Add("Config не соответствует модели");
            }

            if (gcm != null && gs != null && gcm.TaskGrid?.Count < gs.TeamIds?.Count)
                errors.Add("Количество участвующих команд больше, чем указано в сетке");

            return errors.Count == 0;
        }
    }
}
