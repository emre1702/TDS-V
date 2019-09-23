using System.Xml.Serialization;
using TDS_Common.Dto.Map;

namespace TDS_Server.Dto.Map
{
    #nullable disable warnings
    public class MapTeamSpawnsDto
    {
        [XmlIgnore]
        public uint TeamID { get; set; }

        [XmlElement("spawn")]
        public Position4DDto[] Spawns { get; set; }
    }
    #nullable restore warnings
}