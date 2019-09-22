using System;
using System.Xml.Serialization;
using TDS_Common.Dto.Map.Creator;

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

        public MapObjectPosition(MapCreatorPosition obj)
        {
            Name = obj.Info?.ToString() ?? "?";
            X = obj.PosX;
            Y = obj.PosY;
            Z = obj.PosZ;
            RotX = obj.RotX;
            RotY = obj.RotY;
            RotZ = obj.RotZ;
        }

        public MapCreatorPosition ToMapCreatorPosition(int id)
        {
            return new MapCreatorPosition
            {
                Id = id,
                Info = Name,
                PosX = X,
                PosY = Y,
                PosZ = Z,
                RotX = RotX,
                RotY = RotY,
                RotZ = RotZ
            };
        }
    }
}
