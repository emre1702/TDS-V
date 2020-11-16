using Newtonsoft.Json;

namespace TDS.Server.Data.Models.Userpanel.Application
{
    public class ApplicationUserInvitationData
    {
        [JsonProperty("1")]
        public string AdminName { get; set; }

        [JsonProperty("2")]
        public string AdminSCName { get; set; }

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("3")]
        public string Message { get; set; }
    }
}
