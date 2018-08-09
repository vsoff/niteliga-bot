using NiteLigaLibrary.Database;
using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServer.Classes;

namespace WebServer.Controllers
{
    [Authorize]
    public class SocialController : Controller
    {
        // GET: Social/Player/{id}
        public ActionResult Player(int id = 0)
        {
            Player player = null;

            if (id == 0)
            {
                long vkId = InfoManager.GetCurrentUserVkId();
                using (var db = new NiteLigaContext())
                    player = db.Players.FirstOrDefault(x => x.VkId == vkId);
            }
            else
            {
                using (var db = new NiteLigaContext())
                    player = db.Players.FirstOrDefault(x => x.Id == id);
            }
            
            ViewBag.Player = player;

            return View();
        }

        // GET: Social/Team/{id}
        public ActionResult Team(int id = 0)
        {
            Team team = null;
            List<Player> teamPlayers = null;

            using (var db = new NiteLigaContext()) { 
                team = db.Teams.FirstOrDefault(x => x.Id == id);
                teamPlayers = db.PlayersInTeams.Where(x => x.TeamId == id).Select(x => x.Player).ToList();
            }

            ViewBag.Team = team;
            ViewBag.Players = teamPlayers;

            return View();
        }
    }
}