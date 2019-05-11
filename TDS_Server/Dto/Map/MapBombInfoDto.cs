using System.Xml.Serialization;
using TDS_Common.Dto.Map;

namespace TDS_Server.Dto.Map
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized.

    public class MapBombInfoDto
    {
        [XmlElement("plantpos")]
        public MapPositionDto[] PlantPositions { get; set; }

        [XmlIgnore]
        public string PlantPositionsJson { get; set; }
    }

#pragma warning enable CS8618 // Non-nullable field is uninitialized.
}