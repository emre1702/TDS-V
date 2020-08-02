using Newtonsoft.Json;

namespace TDS_Server.Data.Models.GangWindow
{
    public class GangMainData
    {
        [JsonProperty("0")]
        public SyncedGangData GangData { get; set; }

        [JsonProperty("1")]
        public SyncedPlayerGangData PlayerGangData { get; set; }

        [JsonProperty("2")]
        public short HighestRank { get; set; }
    }
}
