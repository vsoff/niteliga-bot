using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServer.Models;

namespace WebServer.Classes
{
    public class InfoManager
    {
        private static ApplicationUser getCurrentApplicationUser()
        {
            return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.Current.User.Identity.GetUserId());
        }

        public static long GetCurrentUserVkId()
        {
            return long.Parse(getCurrentApplicationUser().Logins.First(x => x.LoginProvider == "VKontakte").ProviderKey);
        }
    }
}