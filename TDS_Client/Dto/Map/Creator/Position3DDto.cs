using Newtonsoft.Json;
using TDS_Common.Dto.Map.Creator;

namespace TDS_Client.Dto.Map
{
    public class Position3DDto
    {
        [JsonProperty("0")]
        public float X { get; set; }

        [JsonProperty("1")]
        public float Y { get; set; }

        [JsonProperty("2")]
        public float Z { get; set; }

        public Position3DDto() { }

        public Position3DDto(MapCreatorPosition pos)
        {
            X = pos.PosX;
            Y = pos.PosY;
            Z = pos.PosZ;
        }

        public MapCreatorPosition ToMapCreatorPosition(int id)
        {
            return new MapCreatorPosition
            {
                Id = id,
                PosX = X,
                PosY = Y,
                PosZ = Z
            };
        }

        public override string ToString()
        {
            return $"{X} - {Y} - {Z}";
        }
    }
}
