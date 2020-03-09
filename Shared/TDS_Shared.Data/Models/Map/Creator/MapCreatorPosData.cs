using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.Map.Creator
{
    public class MapCreatorPosData
    {
        [JsonProperty("0")]
        public int Id { get; set; }

        [JsonProperty("1")]
        public float PosX { get; set; }
        [JsonProperty("2")]
        public float PosY { get; set; }
        [JsonProperty("3")]
        public float PosZ { get; set; }
        [JsonProperty("4")]
        public float RotX { get; set; }
        [JsonProperty("5")]
        public float RotY { get; set; }
        [JsonProperty("6")]
        public float RotZ { get; set; }
    }
}
