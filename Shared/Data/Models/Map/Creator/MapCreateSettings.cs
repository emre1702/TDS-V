using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.Map.Creator
{
    public class MapCreateSettings
    {
        [JsonProperty("0")]
        public uint MinPlayers { get; set; }
        [JsonProperty("1")]
        public uint MaxPlayers { get; set; }
    }
}
