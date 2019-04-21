using System.Xml.Serialization;

namespace TDS_Common.Dto.Map
{
    public class MapPositionDto
    {
        [XmlAttribute("x")]
        public float X { get; set; }

        [XmlAttribute("y")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        public float? Z { get; set; }

        [XmlAttribute("rot")]
        public float? Rotation { get; set; }
    }
}
