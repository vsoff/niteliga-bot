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
        private const int ROWSATPAGE = 20;

        // GET: Social/Player/{id?}
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

        // GET: Social/TeamList/{id?}
        public ActionResult TeamList(int id = 1)
        {
            int pageIndex = Math.Max(id, 1);
            List<Team> teams = null;
            int teamsCount;

            using (var db = new NiteLigaContext())
            {
                teams = db.Teams.OrderBy(x => x.Id).Skip((pageIndex - 1) * ROWSATPAGE).Take(ROWSATPAGE).ToList();
                teamsCount = db.Teams.Count();
            }

            ViewBag.Teams = teams;
            ViewBag.TeamsCount = teamsCount;
            ViewBag.PageCount = (int)((double)teamsCount / ROWSATPAGE) + 1;
            ViewBag.PageIndex = pageIndex;

            return View();
        }

        // GET: Social/PlayerList/{id?}
        public ActionResult PlayerList(int id = 1)
        {
            int pageIndex = Math.Max(id, 1);
            List<Player> players = null;
            int playersCount;

            using (var db = new NiteLigaContext())
            {
                players = db.Players.OrderBy(x => x.Id).Skip((pageIndex - 1) * ROWSATPAGE).Take(ROWSATPAGE).ToList();
                playersCount = db.Players.Count();
            }

            ViewBag.Players = players;
            ViewBag.PlayersCount = playersCount;
            ViewBag.PageCount = (int)((double)playersCount / ROWSATPAGE) + 1;
            ViewBag.PageIndex = pageIndex;

            return View();
        }
    }
}