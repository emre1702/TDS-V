using Newtonsoft.Json;

namespace TDS.Shared.Data.Models
{
    public class SyncedPlayerSettings
    {
        [JsonProperty("0")]
        public SyncedClientPlayerSettings Client { get; set; }

        [JsonProperty("1")]
        public string AngularJson { get; set; }     // SyncedAngularPlayerSettings
    }
}
