using System.Xml.Serialization;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map
{
    public class MapObjectPosition
    {

        public MapObjectPosition()
        {
        }

        public MapObjectPosition(MapCreatorPosition pos)
        {
            if (pos is null)
                return;
            Name = pos.Info?.ToString() ?? "?";
            X = pos.PosX;
            Y = pos.PosY;
            Z = pos.PosZ;
            RotX = pos.RotX;
            RotY = pos.RotY;
            RotZ = pos.RotZ;
        }

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("rotX")]
        public float RotX { get; set; }

        [XmlAttribute("rotY")]
        public float RotY { get; set; }

        [XmlAttribute("rotZ")]
        public float RotZ { get; set; }

        [XmlAttribute("x")]
        public float X { get; set; }

        [XmlAttribute("y")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        public float Z { get; set; }

        public MapCreatorPosition ToMapCreatorPosition(int id, MapCreatorPositionType type)
        {
            return new MapCreatorPosition
            {
                Id = id,
                Info = Name,
                Type = type,
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
