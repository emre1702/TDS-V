using System.Xml.Serialization;
using TDS_Server.Enums;

namespace TDS_Server.Data.Models.Map
{

#nullable disable warnings
    public class MapInfoDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public MapType Type { get; set; } = MapType.Normal;

        [XmlAttribute("minplayers")]
        public uint MinPlayers { get; set; } = 0;

        [XmlAttribute("maxplayers")]
        public uint MaxPlayers { get; set; } = uint.MaxValue;

        [XmlElement("creatorid")]
        public int? CreatorId { get; set; }

        [XmlIgnore]
        public string FilePath { get; set; }

        [XmlIgnore]
        public bool IsNewMap { get; set; }
    }
#nullable restore warnings
}