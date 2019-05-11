using System.Xml.Serialization;
using TDS_Common.Dto.Map;

namespace TDS_Server.Dto.Map
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized.

    public class MapTeamSpawnsDto
    {
        [XmlIgnore]
        public uint TeamID { get; set; }

        [XmlElement("spawn")]
        public MapPositionDto[] Spawns { get; set; }
    }

#pragma warning enable CS8618 // Non-nullable field is uninitialized.
}