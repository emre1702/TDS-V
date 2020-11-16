using Newtonsoft.Json;

namespace TDS.Server.Data.Models.Userpanel.Support
{
    public class SupportRequestMessage
    {
        [JsonProperty("0")]
        public string Author { get; set; }

        [JsonProperty("1")]
        public string Message { get; set; }

        [JsonProperty("2")]
        public string CreateTime { get; set; }
    }
}
