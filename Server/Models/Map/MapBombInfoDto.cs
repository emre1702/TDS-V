using System.Xml.Serialization;
using TDS_Common.Dto.Map;

namespace TDS_Server.Dto.Map
{

#nullable disable warnings
    public class MapBombInfoDto
    {
        [XmlElement("plantpos")]
        public Position3DDto[] PlantPositions { get; set; }

        [XmlIgnore]
        public string PlantPositionsJson { get; set; }
    }
#nullable restore warnings

}