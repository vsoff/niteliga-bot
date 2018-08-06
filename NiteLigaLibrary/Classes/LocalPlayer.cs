using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class LocalPlayer
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public long? VkId { get; set; }
        public long? TelegramId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public PlayerStatistic Statictic { get; set; }

        public LocalPlayer(Player player, int teamId)
        {
            this.Id = player.Id;
            this.TeamId = teamId;
            this.VkId = player.VkId;
            this.TelegramId = player.TelegramId;
            this.FirstName = player.FirstName;
            this.LastName = player.LastName;
        }

        public void SendMessage(MessageManager noticer, string text)
        {
            noticer.AddOutputMessage(new Message(this, text));
        }
    }
}
