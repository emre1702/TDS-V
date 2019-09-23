using System.Xml.Serialization;
using TDS_Common.Dto.Map;

namespace TDS_Server.Dto.Map
{

#nullable disable warnings
    public class MapLimitInfoDto
    {
        [XmlElement("pos")]
        public Position3DDto[]? Edges { get; set; }

        [XmlElement("center")]
        public Position3DDto? Center { get; set; }

        [XmlIgnore]
        public string EdgesJson { get; set; }
    }
#nullable restore warnings
}