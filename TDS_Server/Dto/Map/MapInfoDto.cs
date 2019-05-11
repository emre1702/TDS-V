using System.Xml.Serialization;
using TDS_Common.Enum;

namespace TDS_Server.Dto.Map
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized.

    public class MapInfoDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public EMapType Type { get; set; } = EMapType.Normal;

        [XmlAttribute("minplayers")]
        public uint MinPlayers { get; set; } = 0;

        [XmlAttribute("maxplayers")]
        public uint MaxPlayers { get; set; } = uint.MaxValue;

        [XmlElement("creatorid")]
        public int? CreatorId { get; set; }
    }

#pragma warning enable CS8618 // Non-nullable field is uninitialized.
}