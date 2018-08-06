using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class LocalTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LocalPlayer> Players { get; set; }
        public TeamGameProgress Progress { get; set; }

        public LocalTeam(NiteLigaContext db, int teamId)
        {
            Team team = db.Teams.First(x => x.Id == teamId);
            this.Id = team.Id;
            this.Name = team.Name;
            this.Progress = null;
            this.Players = new List<LocalPlayer>();

            List<Player> players = db.PlayersInTeams.Where(x => x.TeamId == team.Id).Select(x => x.Player).ToList();
            for (int i = 0; i < players.Count; i++)
                this.Players.Add(new LocalPlayer(players[i], teamId));
        }

        public void SendMessage(MessageManager noticer, string text)
        {
            noticer.AddOutputMessages(Players, text);
        }
    }
}
