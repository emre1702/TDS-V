using Newtonsoft.Json;
using System.Xml.Serialization;
using TDS_Shared.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map.Creator
{
    #nullable enable
    public class Position4DDto
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

        [XmlAttribute("rot")]
        [JsonProperty("3")]
        public float Rotation { get; set; }

        public Position4DDto() { }

        public Position4DDto(MapCreatorPosition pos)
        {
            X = pos.PosX;
            Y = pos.PosY;
            Z = pos.PosZ;
            Rotation = pos.RotZ;
        }

        public Position3DDto To3D()
        {
            return new Position3DDto { X = X, Y = Y, Z = Z };
        }

        public MapCreatorPosition ToMapCreatorPosition(int id, object? info = null)
        {
            return new MapCreatorPosition
            {
                Id = id,
                Info = info,
                PosX = X,
                PosY = Y,
                PosZ = Z,
                RotZ = Rotation
            };
        }
    }
}
