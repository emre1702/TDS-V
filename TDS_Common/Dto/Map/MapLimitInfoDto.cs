using System.Xml.Serialization;

namespace TDS_Common.Dto.Map
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
    public class MapLimitInfoDto
    {
        [XmlArray("pos")]
        public MapPositionDto[] Edges { get; set; }

        [XmlElement("center")]
        public MapPositionDto? Center { get; set; }

        [XmlIgnore]
        public string EdgesJson { get; set; }
    }
#pragma warning enable CS8618 // Non-nullable field is uninitialized.
}
