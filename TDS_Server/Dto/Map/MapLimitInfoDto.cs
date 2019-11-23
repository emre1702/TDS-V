using MessagePack;
using System.Xml.Serialization;
using TDS_Common.Dto.Map;

namespace TDS_Server.Dto.Map
{

#nullable disable warnings
    [MessagePackObject]
    public class MapLimitInfoDto
    {
        [XmlElement("pos")]
        [Key(0)]
        public Position3DDto[]? Edges { get; set; }

        [XmlElement("center")]
        [Key(1)]
        public Position3DDto? Center { get; set; }

        [XmlIgnore]
        [IgnoreMember]
        public string EdgesJson { get; set; }
    }
#nullable restore warnings
}