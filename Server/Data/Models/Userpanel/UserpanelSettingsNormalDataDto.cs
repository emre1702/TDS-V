using Newtonsoft.Json;
using TDS_Server.Database.Entity.Player.Settings;

namespace TDS_Server.Data.Models.Userpanel
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
