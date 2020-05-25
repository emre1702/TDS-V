using Newtonsoft.Json;

namespace TDS_Server.Data.Models.Userpanel
{
    public class UserpanelSettingsSpecialDataDto
    {
        #region Public Properties

        [JsonProperty("1")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("0")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("2")]
        public bool UsernameBuyInCooldown { get; set; }

        #endregion Public Properties
    }
}
