using System.Xml.Serialization;
using TDS_Server.Dto.Map.Creator;

namespace TDS_Common.Dto.Map
{
    public class Position4DDto
    {
        [XmlAttribute("x")]
        public float X { get; set; }

        [XmlAttribute("y")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        public float Z { get; set; }

        [XmlAttribute("rot")]
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
    }
}