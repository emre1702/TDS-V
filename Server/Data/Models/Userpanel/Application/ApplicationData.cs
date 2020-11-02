using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Server.Data.Models.Userpanel.Stats;

namespace TDS_Server.Data.Models.Userpanel.Application
{
    public class ApplicationData
    {
        [JsonProperty("0")]
        public int ApplicationID { get; set; }

        [JsonProperty("1")]
        public Dictionary<int, string> Answers { get; set; } = new Dictionary<int, string>();

        [JsonProperty("2")]
        public string Questions { get; set; } = string.Empty;

        [JsonProperty("3")]
        public PlayerUserpanelGeneralStatsDataDto Stats { get; set; }

        [JsonProperty("4")]
        public bool AlreadyInvited { get; set; }
    }
}
