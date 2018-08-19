using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class VkResponseUsersGet
    {
        [JsonProperty("response")]
        public VkUserData[] Response { get; set; }
    }

    public class VkUserData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("photo_50")]
        public string Photo50 { get; set; }

        [JsonProperty("photo_200")]
        public string Photo200 { get; set; }
    }

    public class VkRequestModel
    {
        [JsonProperty("type")]
        public string UpdateType { get; set; }

        [JsonProperty("group_id")]
        public long GroupId { get; set; }

        [JsonProperty("object")]
        public VkRequestObjectModel Object { get; set; }
    }

    public class VkRequestObjectModel
    {
        // Сложные параметры (необходимо преобразования)

        [JsonProperty("date")]
        private long _VkDate { get; set; }

        [JsonProperty("read_state")]
        private int _VkReadState { get; set; }

        public DateTime Date
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(_VkDate).ToLocalTime(); }
            set { }
        }

        public bool IsRead
        {
            get { return _VkReadState == 0 ? false : true; }
        }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("out")]
        public int Out { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
