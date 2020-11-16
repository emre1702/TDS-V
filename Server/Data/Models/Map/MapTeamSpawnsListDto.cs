using System.Collections.Generic;
using System.Xml.Serialization;

namespace TDS.Server.Data.Models.Map
{
#nullable disable warnings

    public class MapTeamSpawnsListDto
    {
        [XmlElement("team")]
        public List<MapTeamSpawnsDto> TeamSpawns { get; set; }
    }

#nullable restore warnings
}
