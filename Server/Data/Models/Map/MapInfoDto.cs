﻿using System.Xml.Serialization;
using TDS_Server.Data.Enums;

namespace TDS_Server.Data.Models.Map
{
#nullable disable warnings

    public class MapInfoDto
    {

        [XmlElement("creatorid")]
        public int? CreatorId { get; set; }

        [XmlIgnore]
        public string FilePath { get; set; }

        [XmlIgnore]
        public bool IsNewMap { get; set; }

        [XmlAttribute("maxplayers")]
        public uint MaxPlayers { get; set; } = uint.MaxValue;

        [XmlAttribute("minplayers")]
        public uint MinPlayers { get; set; } = 0;

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public MapType Type { get; set; } = MapType.Normal;

    }

#nullable restore warnings
}
