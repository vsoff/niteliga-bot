using Newtonsoft.Json;
using NiteLigaLibrary.Classes;
using NiteLigaLibrary.Database;
using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebServer.Controllers
{
    //[Authorize(Roles = "organizer")]
    public class OrganizerController : Controller
    {
        // GET: Organizer/Games
        public ActionResult Games()
        {
            List<StoredGame> games;

            using (var db = new NiteLigaContext())
                games = db.StoredGames.ToList();

            ViewBag.Games = games;

            return View();
        }

        // GET: Organizer/Editor
        public ActionResult Editor(int id = 0)
        {
            StoredGame game;

            using (var db = new NiteLigaContext())
                game = db.StoredGames.Single(x => x.Id == id);

            ViewBag.Game = game;

            return View();
        }
    }
}