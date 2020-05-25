using Newtonsoft.Json;

namespace TDS_Server.Data.Models.CustomLobby
{
#nullable disable

    public class CustomLobbyTeamData
    {
        #region Public Properties

        [JsonProperty("2")]
        public int BlipColor { get; set; }

        [JsonProperty("1")]
        public string Color { get; set; }

        [JsonProperty("0")]
        public string Name { get; set; }

        // HTML (rgba(...))
        [JsonProperty("3")]
        public int SkinHash { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
