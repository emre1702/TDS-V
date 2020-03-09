using Newtonsoft.Json;
using TDS_Common.Enum;

namespace TDS_Shared.Data.Models.Map.Creator
{
    public class MapCreatorPosition
    {
        [JsonProperty("0")]
        public int Id { get; set; }
        [JsonProperty("1")]
        public MapCreatorPositionType Type { get; set; }
        [JsonProperty("2")]
        public object Info { get; set; }
        [JsonProperty("3")]
        public float PosX { get; set; }
        [JsonProperty("4")]
        public float PosY { get; set; }
        [JsonProperty("5")]
        public float PosZ { get; set; }
        [JsonProperty("6")]
        public float RotX { get; set; }
        [JsonProperty("7")]
        public float RotY { get; set; }
        [JsonProperty("8")]
        public float RotZ { get; set; }

        [JsonProperty("9")]
        public ushort OwnerRemoteId { get; set; }

        public MapCreatorPosition() { }
    }
}
