using Newtonsoft.Json;

namespace TDS.Server.Data.Models.Userpanel.Application
{
    public class AppToSendData
    {
        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("1")]
        public string CreateTime { get; set; }

        [JsonProperty("2")]
        public string PlayerName { get; set; }

    }
}
