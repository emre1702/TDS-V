using Newtonsoft.Json;
using System;

namespace TDS_Server.Data.Models.Userpanel.History
{
    public class PlayerUserpanelAdminTargetHistoryDataDto
    {
        [JsonProperty("0")]
        public string Admin { get; set; }

        [JsonProperty("3")]
        public bool AsDonator { get; set; }

        [JsonProperty("4")]
        public bool AsVip { get; set; }

        [JsonProperty("7")]
        public string LengthOrEndTime { get; set; }

        [JsonProperty("1")]
        public string Lobby { get; set; }

        [JsonIgnore]
        public int? LobbyId { get; set; }

        [JsonProperty("5")]
        public string Reason { get; set; }

        [JsonIgnore]
        public int SourceId { get; set; }

        [JsonIgnore]
        public int? TargetId { get; set; }

        [JsonProperty("6")]
        public string Timestamp { get; set; }

        [JsonIgnore]
        public DateTime TimestampDateTime { get; set; }

        [JsonProperty("2")]
        public string Type { get; set; }
    }
}
