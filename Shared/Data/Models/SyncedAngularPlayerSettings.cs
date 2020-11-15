using Newtonsoft.Json;

namespace TDS_Shared.Data.Models
{
    public class SyncedAngularPlayerSettings
    {
        [JsonProperty("0")]
        public float ChatWidth { get; set; }

        [JsonProperty("1")]
        public float ChatMaxHeight { get; set; }

        [JsonProperty("2")]
        public float ChatFontSize { get; set; }

        [JsonProperty("3")]
        public bool HideDirtyChat { get; set; }

        [JsonProperty("4")]
        public bool HideChatInfo { get; set; }

        [JsonProperty("5")]
        public float ChatInfoFontSize { get; set; }

        [JsonProperty("6")]
        public int ChatInfoMoveTimeMs { get; set; }

        [JsonProperty("7")]
        public bool KillInfoShowIcon { get; set; }

        [JsonProperty("8")]
        public float KillInfoFontSize { get; set; }

        [JsonProperty("9")]
        public int KillInfoIconWidth { get; set; }

        [JsonProperty("10")]
        public int KillInfoSpacing { get; set; }

        [JsonProperty("11")]
        public float KillInfoDuration { get; set; }

        [JsonProperty("12")]
        public int KillInfoIconHeight { get; set; }

        [JsonProperty("13")]
        public bool UseDarkTheme { get; set; }

        [JsonProperty("14")]
        public string ThemeMainColor { get; set; }

        [JsonProperty("15")]
        public string ThemeSecondaryColor { get; set; }

        [JsonProperty("16")]
        public string ThemeWarnColor { get; set; }

        [JsonProperty("17")]
        public string ThemeBackgroundDarkColor { get; set; }

        [JsonProperty("18")]
        public string ThemeBackgroundLightColor { get; set; }

        [JsonProperty("19")]
        public int ToolbarDesign { get; set; }
    }
}
