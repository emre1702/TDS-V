using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.FakePickup
{
    public class FakePickupLightData
    {
        [JsonProperty("0")]
        public byte Red { get; set; }

        [JsonProperty("1")]
        public byte Green { get; set; }

        [JsonProperty("2")]
        public byte Blue { get; set; }

        [JsonProperty("3")]
        public float Range { get; set; }

        [JsonProperty("4")]
        public float Intensity { get; set; }

        [JsonProperty("5")]
        public float Shadow { get; set; }
    }
}
