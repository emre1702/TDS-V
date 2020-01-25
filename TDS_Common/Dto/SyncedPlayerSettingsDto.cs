using Newtonsoft.Json;
using TDS_Common.Enum;

namespace TDS_Common.Dto
{
    public class SyncedPlayerSettingsDto
    {
        [JsonProperty("0")]
        public int PlayerId { get; set; }

        #region General 
        [JsonProperty("1")]
        public ELanguage Language { get; set; }
        [JsonProperty("2")]
        public bool AllowDataTransfer { get; set; }
        [JsonProperty("3")]
        public bool ShowConfettiAtRanking { get; set; }
        [JsonProperty("4")]
        public string Timezone { get; set; }
        [JsonProperty("13")]
        public string DateTimeFormat { get; set; }
        [JsonProperty("5")]
        public ulong DiscordUserId { get; set; }
        #endregion

        #region Fight
        [JsonProperty("6")]
        public bool Hitsound { get; set; }
        [JsonProperty("7")]
        public bool Bloodscreen { get; set; }
        [JsonProperty("8")]
        public bool FloatingDamageInfo { get; set; }
        #endregion

        #region Voice
        [JsonProperty("9")]
        public bool Voice3D { get; set; }
        [JsonProperty("10")]
        public bool VoiceAutoVolume { get; set; }
        [JsonProperty("11")]
        public float VoiceVolume { get; set; }
        #endregion

        #region Graphical 
        [JsonProperty("12")]
        public string MapBorderColor { get; set; }
        #endregion
    }
}
