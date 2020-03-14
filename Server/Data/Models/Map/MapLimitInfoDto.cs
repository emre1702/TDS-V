using Newtonsoft.Json;
using System.Xml.Serialization;
using TDS_Server.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map
{
    #nullable enable
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
        public string EdgesJson { get; set; } = "[]";
    }
}
