using Newtonsoft.Json;

namespace TDS_Shared.Data.Models
{
    public class SyncedServerSettingsDto
    {
        [JsonProperty("0")]
        public float DistanceToSpotToPlant;
        [JsonProperty("1")]
        public float DistanceToSpotToDefuse;
        [JsonProperty("2")]
        public int RoundEndTime;
        [JsonProperty("3")]
        public int MapChooseTime;
        [JsonProperty("4")]
        public int TeamOrderCooldownMs;
        [JsonProperty("5")]
        public float NametagMaxDistance;
        [JsonProperty("6")]
        public bool ShowNametagOnlyOnAiming;
        [JsonProperty("7")]
        public int ArenaLobbyId;
        [JsonProperty("8")]
        public int MapCreatorLobbyId;
        [JsonProperty("9")]
        public int CharCreatorLobbyId;
    }
}
