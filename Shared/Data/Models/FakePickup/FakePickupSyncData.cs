using Newtonsoft.Json;

namespace TDS.Shared.Data.Models.FakePickup
{
    public class FakePickupSyncData
    {
        public ushort RemoteId { get; set; }
        public string LightDataJson { get; set; }

        [JsonIgnore]
        public FakePickupLightData LightData { get; set; } 
    }
}
