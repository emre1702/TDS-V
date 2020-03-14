using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.Map
{
    public class Position3DDto
    {
        [JsonProperty("0")]
        public float X { get; set; }

        [JsonProperty("1")]
        public float Y { get; set; }

        [JsonProperty("2")]
        public float Z { get; set; }
    }
}
