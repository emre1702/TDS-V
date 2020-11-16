using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;
using TDS.Server.Data.Models.Map.Creator;

namespace TDS.Server.Data.Models.Map
{
#nullable enable

    public class MapLimitInfoDto
    {

        [XmlElement("center")]
        [JsonProperty("1")]
        public Position3DDto? Center { get; set; }

        [XmlElement("pos")]
        [JsonProperty("0")]
        public List<Position3DDto>? Edges { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string EdgesJson { get; set; } = "[]";

    }
}
