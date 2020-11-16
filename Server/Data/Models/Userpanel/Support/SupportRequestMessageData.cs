using Newtonsoft.Json;
using System;

namespace TDS.Server.Data.Models.Userpanel.Support
{
    public class SupportRequestMessageData
    {
        [JsonProperty("0")]
        public string Author { get; set; }

        [JsonProperty("2")]
        public string CreateTime { get; set; }

        [JsonIgnore]
        public DateTime CreateTimeDate { get; set; }

        [JsonProperty("1")]
        public string Message { get; set; }
    }
}
