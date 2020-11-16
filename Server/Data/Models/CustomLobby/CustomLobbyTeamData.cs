using Newtonsoft.Json;

namespace TDS.Server.Data.Models.CustomLobby
{
#nullable disable

    public class CustomLobbyTeamData
    {

        [JsonProperty("2")]
        public int BlipColor { get; set; }

        [JsonProperty("1")]
        public string Color { get; set; }

        [JsonProperty("0")]
        public string Name { get; set; }

        // HTML (rgba(...))
        [JsonProperty("3")]
        public int SkinHash { get; set; }

    }

#nullable restore
}
