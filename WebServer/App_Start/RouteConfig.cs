using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebServer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Player",
                "Player/{id}",
                new { action = "Player", controller = "Social", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Team",
                "Team/{id}",
                new { action = "Team", controller = "Social" }
            );

            routes.MapRoute(
                "TeamList",
                "TeamList/{id}",
                new { action = "TeamList", controller = "Social", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "PlayerList",
                "PlayerList/{id}",
                new { action = "PlayerList", controller = "Social", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
