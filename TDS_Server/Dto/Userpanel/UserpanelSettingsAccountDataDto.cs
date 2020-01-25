using Newtonsoft.Json;

namespace TDS_Server.Dto.Userpanel
{
    public class UserpanelSettingsSpecialDataDto
    {
        [JsonProperty("0")]
        public string Username { get; set; } = string.Empty;
        
        [JsonProperty("1")]
        public string Email { get; set; } = string.Empty;
    }
}
