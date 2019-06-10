using GTANetworkAPI;
using System.Xml.Serialization;

namespace TDS_Common.Dto.Map
{
    public class Position3DDto
    {
        [XmlAttribute("x")]
        public float X { get; set; }

        [XmlAttribute("y")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        public float Z { get; set; }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}