using NiteLigaLibrary.Classes;
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
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<PlayerInTeam> PlayersInTeams { get; set; }

        /// <summary>
        /// Добавляет новых пользователей в базу по VK id (если пользователей несколько, то их нужно указывать через запятую)
        /// </summary>
        public static void AddNewUsers(string userVkIds)
        {
            var vkData = VkontakteManager.GetUserData(userVkIds);

            if (vkData.Response.Length > 0)
                using (var db = new NiteLigaContext())
                {
                    foreach (VkUserData user in vkData.Response)
                    {
                        if (db.Players.FirstOrDefault(x => x.VkId == user.Id) == null)
                            db.Players.Add(new Player
                            {
                                VkId = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Photo50 = user.Photo50,
                                Photo200 = user.Photo200,
                                RegistrationDate = DateTime.Now
                            });
                    }
                    db.SaveChanges();
                }
        }
    }
}