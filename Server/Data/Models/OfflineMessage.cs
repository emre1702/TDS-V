using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Data.Models
{
    public class OfflineMessage
    {
        [JsonProperty("0")]
        public int ID { get; set; }
        [JsonProperty("1")]
        public string PlayerName { get; set; } = string.Empty;
        [JsonProperty("2")]
        public string CreateTime { get; set; } = string.Empty;
        [JsonProperty("3")]
        public string Text { get; set; } = string.Empty;
        [JsonProperty("4")]
        public bool Seen { get; set; }

        [JsonIgnore]
        public DateTime CreateTimeDate { get; set; }
    }
}
