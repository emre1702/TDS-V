using System.Xml.Serialization;
using TDS_Common.Enum;

namespace TDS_Common.Dto.Map
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
    public class MapInfoDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public EMapType Type { get; set; } = EMapType.Normal;

        [XmlAttribute("minplayers")]
        public int MinPlayers { get; set; } = int.MinValue;

        [XmlAttribute("maxplayers")]
        public int MaxPlayers { get; set; } = int.MaxValue;

        [XmlAttribute("creatorid")]
        public uint? CreatorId { get; set; }
    }
#pragma warning enable CS8618 // Non-nullable field is uninitialized.
}
