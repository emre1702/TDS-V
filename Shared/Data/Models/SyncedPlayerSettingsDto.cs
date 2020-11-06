using Newtonsoft.Json;

namespace TDS_Shared.Data.Models
{
    public class SyncedPlayerSettingsDto
    {

        [JsonProperty("17")]
        public int AFKKickAfterSeconds { get; set; }

        [JsonProperty("18")]
        public int AFKKickShowWarningLastSeconds { get; set; }

        [JsonProperty("2")]
        public bool AllowDataTransfer { get; set; }

        [JsonProperty("7")]
        public bool Bloodscreen { get; set; }

        [JsonProperty("14")]
        public int BloodscreenCooldownMs { get; set; }

        [JsonProperty("29")]
        public float ChatFontSize { get; set; }

        [JsonProperty("36")]
        public float ChatInfoFontSize { get; set; }

        [JsonProperty("37")]
        public int ChatInfoMoveTimeMs { get; set; }

        [JsonProperty("28")]
        public float ChatMaxHeight { get; set; }

        [JsonProperty("27")]
        public float ChatWidth { get; set; }

        [JsonProperty("25")]
        public bool CheckAFK { get; set; }

        [JsonProperty("13")]
        public string DateTimeFormat { get; set; }

        [JsonProperty("5")]
        public ulong? DiscordUserId { get; set; }

        [JsonProperty("8")]
        public bool FloatingDamageInfo { get; set; }

        [JsonProperty("35")]
        public bool HideChatInfo { get; set; }

        [JsonProperty("30")]
        public bool HideDirtyChat { get; set; }

        [JsonProperty("6")]
        public bool Hitsound { get; set; }

        [JsonProperty("15")]
        public int HudAmmoUpdateCooldownMs { get; set; }

        [JsonProperty("16")]
        public int HudHealthUpdateCooldownMs { get; set; }

        [JsonProperty("12")]
        public string MapBorderColor { get; set; }

        [JsonProperty("23")]
        public string NametagArmorEmptyColor { get; set; }

        [JsonProperty("24")]
        public string NametagArmorFullColor { get; set; }

        [JsonProperty("20")]
        public string NametagDeadColor { get; set; }

        [JsonProperty("21")]
        public string NametagHealthEmptyColor { get; set; }

        [JsonProperty("22")]
        public string NametagHealthFullColor { get; set; }

        [JsonProperty("0")]
        public int PlayerId { get; set; }

        [JsonProperty("33")]
        public bool ScoreboardPlayerSortingDesc { get; set; }

        [JsonProperty("34")]
        public Enums.TimeSpanUnitsOfTime ScoreboardPlaytimeUnit { get; set; }

        [JsonProperty("3")]
        public bool ShowConfettiAtRanking { get; set; }

        [JsonProperty("45")]
        public bool ShowCursorInfo { get; set; }

        [JsonProperty("31")]
        public bool ShowCursorOnChatOpen { get; set; }

        [JsonProperty("19")]
        public int ShowFloatingDamageInfoDurationMs { get; set; }

        [JsonProperty("46")]
        public bool ShowLobbyLeaveInfo { get; set; }

        [JsonProperty("4")]
        public string Timezone { get; set; }

        [JsonProperty("9")]
        public bool Voice3D { get; set; }

        [JsonProperty("10")]
        public bool VoiceAutoVolume { get; set; }

        [JsonProperty("11")]
        public float VoiceVolume { get; set; }

        [JsonProperty("26")]
        public bool WindowsNotifications { get; set; }

        [JsonProperty("1")]
        public Enums.Language Language { get; set; }

        [JsonProperty("32")]
        public Enums.ScoreboardPlayerSorting ScoreboardPlayerSorting { get; set; }

    }
}
