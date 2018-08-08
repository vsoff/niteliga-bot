using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebServer.Models;
using Microsoft.AspNet.Identity.Owin;
using WebServer.Classes;
using NiteLigaLibrary.Database;
using NiteLigaLibrary.Database.Models;

namespace WebServer.Controllers
{
    public class PlayerController : Controller
    {
        // GET: Player
        public ActionResult Index()
        {
            long vkId = InfoManager.GetCurrentUserVkId();
            Player pl = null;
            using (var db = new NiteLigaContext())
                pl = db.Players.FirstOrDefault(x => x.VkId == vkId);

            ViewBag.IsUserExists = pl != null;
            ViewBag.PlayerId = pl?.Id;
            ViewBag.VkId = pl?.VkId;
            ViewBag.FirstName = pl?.FirstName;
            ViewBag.LastName = pl?.LastName;

            return View();
        }
    }
}