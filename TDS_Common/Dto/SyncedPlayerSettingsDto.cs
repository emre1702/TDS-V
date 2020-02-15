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
        [JsonProperty("25")]
        public bool CheckAFK { get; set; }
        #endregion

        #region Voice
        [JsonProperty("9")]
        public bool Voice3D { get; set; }
        [JsonProperty("10")]
        public bool VoiceAutoVolume { get; set; }
        [JsonProperty("11")]
        public float VoiceVolume { get; set; }
        #endregion

        #region Colors 
        [JsonProperty("12")]
        public string MapBorderColor { get; set; }
        [JsonProperty("20")]
        public string NametagDeadColor { get; set; }
        [JsonProperty("21")]
        public string NametagHealthEmptyColor { get; set; }
        [JsonProperty("22")]
        public string NametagHealthFullColor { get; set; }
        [JsonProperty("23")]
        public string NametagArmorEmptyColor { get; set; }
        [JsonProperty("24")]
        public string NametagArmorFullColor { get; set; }
        #endregion 

        #region Times
        [JsonProperty("14")]
        public int BloodscreenCooldownMs { get; set; }
        [JsonProperty("15")]
        public int HudAmmoUpdateCooldownMs { get; set; }
        [JsonProperty("16")]
        public int HudHealthUpdateCooldownMs { get; set; }
        [JsonProperty("17")]
        public int AFKKickAfterSeconds { get; set; }
        [JsonProperty("18")]
        public int AFKKickShowWarningLastSeconds { get; set; }
        [JsonProperty("19")]
        public int ShowFloatingDamageInfoDurationMs { get; set; }
        #endregion
    }
}
