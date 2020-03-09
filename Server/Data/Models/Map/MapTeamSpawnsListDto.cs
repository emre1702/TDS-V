using System.Xml.Serialization;

namespace TDS_Server.Data.Models.Map
{
    #nullable disable warnings
    public class MapTeamSpawnsListDto
    {
        [XmlElement("team")]
        public MapTeamSpawnsDto[] TeamSpawns { get; set; }
    }
    #nullable restore warnings
}