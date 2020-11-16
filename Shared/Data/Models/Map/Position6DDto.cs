using Newtonsoft.Json;

namespace TDS.Shared.Data.Models.Map
{
    public class Position6DDto
    {
        [JsonProperty("0")]
        public float X { get; set; }

        [JsonProperty("1")]
        public float Y { get; set; }

        [JsonProperty("2")]
        public float Z { get; set; }

        [JsonProperty("3")]
        public float RotX { get; set; }

        [JsonProperty("4")]
        public float RotY { get; set; }

        [JsonProperty("5")]
        public float RotZ { get; set; }
    }
}
