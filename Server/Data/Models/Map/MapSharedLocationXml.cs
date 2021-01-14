using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TDS.Shared.Data.Interfaces.Map.Creator;
using TDS.Shared.Data.Models.Map.Creator;

namespace TDS.Server.Data.Models.Map
{
    public class MapSharedLocationXml
    {
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("ipls")]
        [XmlArray("ipls", IsNullable = true)]
        [XmlArrayItem("ipl", IsNullable = true)]
        public List<string> Ipls { get; set; }

        [JsonProperty("iplsToUnload")]
        [XmlArray("iplsToUnload", IsNullable = true)]
        [XmlArrayItem("iplToUnload", IsNullable = true)]
        public List<string> IplsToUnload { get; set; }

        public MapSharedLocationXml()
        {
        }

        public MapSharedLocationXml(MapSharedLocation location)
        {
            Name = location.Name;
            Ipls = location.Ipls;
            IplsToUnload = location.IplsToUnload;
        }
    }
}