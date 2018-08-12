using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebServer.Controllers
{
    public class HomeController : Controller
    {
        public static DateTime ServerStartupTime { get; } = DateTime.Now;

        public ActionResult Index()
        {
            ViewBag.ServerStartupTime = ServerStartupTime;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}