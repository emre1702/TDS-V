using GTANetworkAPI;
using Newtonsoft.Json;
using System.Xml.Serialization;
using TDS_Common.Dto.Map.Creator;

namespace TDS_Server.Dto.Map
{
    public class Position3DDto
    {
        [XmlAttribute("x")]
        [JsonProperty("0")]
        public float X { get; set; }

        [XmlAttribute("y")]
        [JsonProperty("1")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        [JsonProperty("2")]
        public float Z { get; set; }

        public Position3DDto() { }

        public Position3DDto(MapCreatorPosition pos)
        {
            X = pos.PosX;
            Y = pos.PosY;
            Z = pos.PosZ;
        }

        public Position3DDto(Vector3 pos)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
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
    }
}
