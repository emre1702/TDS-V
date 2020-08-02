using Newtonsoft.Json;

namespace TDS_Server.Data.Models.GangWindow
{
    public class SyncedPlayerGangData
    {
        [JsonProperty("0")]
        public int Rank { get; set; }

        [JsonProperty("1")]
        public bool IsGangOwner { get; set; }
    }
}
