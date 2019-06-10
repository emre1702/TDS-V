using System.Xml.Serialization;

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

        public Position3DDto To3D()
        {
            return new Position3DDto { X = X, Y = Y, Z = Z };
        }
    }
}