using Newtonsoft.Json;

namespace TDS_Server.Dto.CustomLobby
{
    #nullable disable
    public class CustomLobbyTeamData
    {
        [JsonProperty("0")]
        public string Name { get; set; }
        [JsonProperty("1")]
        public string Color { get; set; }   // HTML (rgba(...))
        [JsonProperty("2")]
        public int BlipColor { get; set; }
        [JsonProperty("3")]
        public int SkinHash { get; set; }
    }
    #nullable restore
}
