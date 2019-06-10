using System.Xml.Serialization;
using TDS_Common.Dto.Map;

namespace TDS_Server.Dto.Map
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized.

    public class MapLimitInfoDto
    {
        [XmlElement("pos")]
        public Position3DDto[] Edges { get; set; }

        [XmlElement("center")]
        public Position3DDto? Center { get; set; }

        [XmlIgnore]
        public string EdgesJson { get; set; }
    }

#pragma warning enable CS8618 // Non-nullable field is uninitialized.
}