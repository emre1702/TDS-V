using Newtonsoft.Json;

namespace TDS_Shared.Data.Models
{
    public class SyncedPlayerThemeSettings
    {

        [JsonProperty("1004")]
        public string ThemeBackgroundDarkColor { get; set; }

        [JsonProperty("1005")]
        public string ThemeBackgroundLightColor { get; set; }

        [JsonProperty("1001")]
        public string ThemeMainColor { get; set; }

        [JsonProperty("1002")]
        public string ThemeSecondaryColor { get; set; }

        [JsonProperty("1003")]
        public string ThemeWarnColor { get; set; }

        [JsonProperty("1006")]
        public int ToolbarDesign { get; set; }

        [JsonProperty("1000")]
        public bool UseDarkTheme { get; set; }

    }
}
