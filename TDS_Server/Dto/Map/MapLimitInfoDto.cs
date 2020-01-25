using Newtonsoft.Json;
using System.Xml.Serialization;

namespace TDS_Server.Dto.Map
{

#nullable disable warnings
    public class MapLimitInfoDto
    {
        [XmlElement("pos")]
        [JsonProperty("0")]
        public Position3DDto[]? Edges { get; set; }

        [XmlElement("center")]
        [JsonProperty("1")]
        public Position3DDto? Center { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string EdgesJson { get; set; }
    }
#nullable restore warnings
}
