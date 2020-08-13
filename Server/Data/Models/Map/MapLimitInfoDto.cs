using Newtonsoft.Json;
using System.Xml.Serialization;
using TDS_Server.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map
{
#nullable enable

    public class MapLimitInfoDto
    {
        #region Public Properties

        [XmlElement("center")]
        [JsonProperty("1")]
        public PositionDto? Center { get; set; }

        [XmlElement("pos")]
        [JsonProperty("0")]
        public PositionDto[]? Edges { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string EdgesJson { get; set; } = "[]";

        #endregion Public Properties
    }
}
