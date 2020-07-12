﻿using Newtonsoft.Json;

namespace TDS_Shared.Data.Models
{
    public class SyncedPlayerThemeSettings
    {
        #region Public Properties

        [JsonProperty("1")]
        public float ThemeBackgroundAlphaPercentage { get; set; }

        [JsonProperty("5")]
        public string ThemeBackgroundDarkColor { get; set; }

        [JsonProperty("6")]
        public string ThemeBackgroundLightColor { get; set; }

        [JsonProperty("2")]
        public string ThemeMainColor { get; set; }

        [JsonProperty("3")]
        public string ThemeSecondaryColor { get; set; }

        [JsonProperty("4")]
        public string ThemeWarnColor { get; set; }

        [JsonProperty("7")]
        public int ToolbarDesign { get; set; }

        [JsonProperty("0")]
        public bool UseDarkTheme { get; set; }

        #endregion Public Properties
    }
}
