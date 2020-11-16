using System.Collections.Generic;
using System.Xml.Serialization;
using TDS.Server.Data.Models.Map.Creator;

namespace TDS.Server.Data.Models.Map
{
    #nullable disable
    public class MapBombInfoDto
    {

        [XmlElement("plantpos")]
        public List<Position3DDto> PlantPositions { get; set; }

        [XmlIgnore]
        public string PlantPositionsJson { get; set; }

    }
}
