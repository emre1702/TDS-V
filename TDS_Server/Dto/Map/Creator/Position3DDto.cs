using MessagePack;
using System.Xml.Serialization;
using TDS_Common.Dto.Map.Creator;

namespace TDS_Server.Dto.Map
{
    [MessagePackObject]
    public class Position3DDto
    {
        [XmlAttribute("x")]
        [Key(0)]
        public float X { get; set; }

        [XmlAttribute("y")]
        [Key(1)]
        public float Y { get; set; }

        [XmlAttribute("z")]
        [Key(2)]
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
    }
}