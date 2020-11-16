using Newtonsoft.Json;

namespace TDS.Server.Data.Models.GangWindow
{
    public class SyncedGangData : GangCreateData
    {
        [JsonProperty("99")]
        public int Id { get; set; }

        [JsonProperty("98")]
        public int OwnerId { get; set; }
    }
}
