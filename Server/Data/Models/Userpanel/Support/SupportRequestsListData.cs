using Newtonsoft.Json;
using System;
using TDS.Shared.Data.Enums.Userpanel;

namespace TDS.Server.Data.Models.Userpanel.Support
{
    public class SupportRequestsListData
    {
        [JsonProperty("5")]
        public bool Closed { get; set; }

        [JsonProperty("2")]
        public string CreateTime { get; set; } 

        [JsonIgnore]
        public DateTime CreateTimeDate { get; set; }

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("1")]
        public string PlayerName { get; set; }

        [JsonProperty("4")]
        public string Title { get; set; }

        [JsonProperty("3")]
        public SupportType Type { get; set; }
    }
}
