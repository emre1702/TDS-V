using Newtonsoft.Json;

namespace TDS.Server.Data.Models
{
    public class RegisterLoginInitData
    {
        [JsonProperty("0")]
        public bool IsRegistered { get; set; }

        [JsonProperty("1")]
        public string Name { get; set; }
    }
}
