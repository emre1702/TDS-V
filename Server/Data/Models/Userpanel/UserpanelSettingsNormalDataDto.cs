using Newtonsoft.Json;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Data.Models.Userpanel
{
    public class UserpanelSettingsNormalDataDto
    {
        #region Public Properties

        [JsonProperty("0")]
        public PlayerSettings General { get; set; }

        [JsonProperty("1")]
        public PlayerThemeSettings ThemeSettings { get; set; }

        #endregion Public Properties
    }
}
