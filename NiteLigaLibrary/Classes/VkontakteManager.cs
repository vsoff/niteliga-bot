using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public static class VkontakteManager
    {
        public static VkResponseUsersGet GetUserData(string id)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                webClient.QueryString.Add("lang", "ru");
                webClient.QueryString.Add("user_ids", id);
                webClient.QueryString.Add("fields", "photo_50%2Cphoto_200");
                webClient.QueryString.Add("access_token", ConfigurationManager.AppSettings["VkApiToken"]);
                webClient.QueryString.Add("v", ConfigurationManager.AppSettings["VkApiVersion"]);

                return JsonConvert.DeserializeObject<VkResponseUsersGet>(webClient.DownloadString("https://api.vk.com/method/users.get"));
            }
        }
    }
}
