using Newtonsoft.Json;

namespace TDS.Shared.Data.Models
{
    public class SyncedServerSettingsDto
    {
        [JsonProperty("0")]
        public float DistanceToSpotToPlant { get; set; }

        [JsonProperty("1")]
        public float DistanceToSpotToDefuse { get; set; }

        [JsonProperty("2")]
        public int RoundEndTime { get; set; }

        [JsonProperty("3")]
        public int MapChooseTime { get; set; }

        [JsonProperty("4")]
        public int TeamOrderCooldownMs { get; set; }

        [JsonProperty("5")]
        public float NametagMaxDistance { get; set; }

        [JsonProperty("6")]
        public bool ShowNametagOnlyOnAiming { get; set; }
    }
}
