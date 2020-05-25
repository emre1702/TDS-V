using Newtonsoft.Json;
using System;

namespace TDS_Server.Data.Models
{
    public class OfflineMessage
    {
        #region Public Properties

        [JsonProperty("2")]
        public string CreateTime { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime CreateTimeDate { get; set; }

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("1")]
        public string PlayerName { get; set; } = string.Empty;

        [JsonProperty("4")]
        public bool Seen { get; set; }

        [JsonProperty("3")]
        public string Text { get; set; } = string.Empty;

        #endregion Public Properties
    }
}
