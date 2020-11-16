using Newtonsoft.Json;
using TDS.Server.Database.Entity.Player.Settings;

namespace TDS.Server.Data.Models.Userpanel
{
    public class UserpanelSettingsNormalDataDto
    {

        [JsonProperty("0")]
        public PlayerSettings General { get; set; }

        [JsonProperty("1")]
        public PlayerThemeSettings ThemeSettings { get; set; }

        [JsonProperty("2")]
        public PlayerKillInfoSettings KillInfoSettings { get; set; }

    }
}
