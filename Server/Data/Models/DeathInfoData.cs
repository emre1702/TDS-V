using Newtonsoft.Json;

namespace TDS_Server.Data.Models
{
    public class DeathInfoData
    {
        [JsonProperty("0")]
        public string PlayerName { get; set; }

        [JsonProperty("1")]
        public string KillerName { get; set; }

        [JsonProperty("2")]
        public uint WeaponHash { get; set; }
    }
}
