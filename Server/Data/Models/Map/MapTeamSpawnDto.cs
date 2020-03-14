using System.Xml.Serialization;
using TDS_Server.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map
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
