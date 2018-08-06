using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class Message
    {
        public DateTime AddDate { get; }
        public LocalPlayer Player { get; set; }
        public string Text { get; set; }

        public Message(LocalPlayer player, string text)
        {
            this.AddDate = DateTime.Now;
            this.Player = player;
            this.Text = text;
        }

        public Message(long vkId, string text)
        {
            this.AddDate = DateTime.Now;
            using (var db = new NiteLigaContext())
            {
                Player player = db.Players.First(x => x.VkId == vkId);
                var a = db.PlayersInTeams.Where(x => x.PlayerId == player.Id);
                var teamId = db.PlayersInTeams.Where(x => x.PlayerId == player.Id).First().TeamId;
                this.Player = new LocalPlayer(player, teamId);
            }
            this.Text = text;
        }
    }
}
