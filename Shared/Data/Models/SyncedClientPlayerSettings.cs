using Newtonsoft.Json;
using TDS.Shared.Data.Enums;

namespace TDS.Shared.Data.Models
{
    public class SyncedClientPlayerSettings
    {
        [JsonProperty("0")]
        public Language Language { get; set; }

        [JsonProperty("1")]
        public string MapBorderColor { get; set; }

        [JsonProperty("2")]
        public string NametagDeadColor { get; set; }

        [JsonProperty("3")]
        public string NametagHealthEmptyColor { get; set; }

        [JsonProperty("4")]
        public string NametagHealthFullColor { get; set; }

        [JsonProperty("5")]
        public string NametagArmorEmptyColor { get; set; }

        [JsonProperty("6")]
        public string NametagArmorFullColor { get; set; }

        [JsonProperty("7")]
        public bool ShowCursorInfo { get; set; }

        [JsonProperty("8")]
        public bool ShowLobbyLeaveInfo { get; set; }

        [JsonProperty("9")]
        public bool VoiceAutoVolume { get; set; }

        [JsonProperty("10")]
        public float VoiceVolume { get; set; }

        [JsonProperty("11")]
        public bool Voice3D { get; set; }

        [JsonProperty("12")]
        public int AFKKickShowWarningLastSeconds { get; set; }

        [JsonProperty("13")]
        public bool CheckAFK { get; set; }

        [JsonProperty("14")]
        public int AFKKickAfterSeconds { get; set; }

        [JsonProperty("15")]
        public bool ShowCursorOnChatOpen { get; set; }

        [JsonProperty("16")]
        public bool Hitsound { get; set; }

        [JsonProperty("17")]
        public bool FloatingDamageInfo { get; set; }

        [JsonProperty("18")]
        public int BloodscreenCooldownMs { get; set; }

        [JsonProperty("19")]
        public int HudHealthUpdateCooldownMs { get; set; }

        [JsonProperty("20")]
        public int HudAmmoUpdateCooldownMs { get; set; }

        [JsonProperty("21")]
        public int ShowFloatingDamageInfoDurationMs { get; set; }

        [JsonProperty("22")]
        public bool ScoreboardPlayerSortingDesc { get; set; }

        [JsonProperty("23")]
        public ScoreboardPlayerSorting ScoreboardPlayerSorting { get; set; }

        [JsonProperty("24")]
        public TimeSpanUnitsOfTime ScoreboardPlaytimeUnit { get; set; }

        [JsonProperty("25")]
        public bool WindowsNotifications { get; set; }

        [JsonProperty("26")]
        public bool ShowConfettiAtRanking { get; set; }

        [JsonProperty("27")]
        public bool Bloodscreen { get; set; }
    }
}
