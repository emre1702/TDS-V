using MessagePack;
using System.Xml.Serialization;
using TDS_Common.Dto.Map.Creator;

namespace TDS_Server.Dto.Map
{
    [MessagePackObject]
    public class Position4DDto
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

        [XmlAttribute("rot")]
        [Key(3)]
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