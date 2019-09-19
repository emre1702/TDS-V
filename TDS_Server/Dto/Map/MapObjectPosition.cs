using System.Xml.Serialization;
using TDS_Server.Dto.Map.Creator;

namespace TDS_Server.Dto.Map
{
    public class MapObjectPosition
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("x")]
        public float X { get; set; }

        [XmlAttribute("y")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        public float Z { get; set; }

        [XmlAttribute("rotX")]
        public float RotX { get; set; }

        [XmlAttribute("rotY")]
        public float RotY { get; set; }

        [XmlAttribute("rotZ")]
        public float RotZ { get; set; }

        public MapObjectPosition() { }

        public MapObjectPosition(MapCreatorObject obj)
        {
            Name = obj.ObjectName;
            X = obj.Position.PosX;
            Y = obj.Position.PosY;
            Z = obj.Position.PosZ;
            RotX = obj.Position.RotX;
            RotY = obj.Position.RotY;
            RotZ = obj.Position.RotZ;
        }
    }
}
