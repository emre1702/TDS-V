﻿using System.Xml.Serialization;
using TDS_Common.Dto.Map;

namespace TDS_Server.Data.Models.Map
{

#nullable disable warnings
    public class MapObjectsListDto
    {
        [XmlElement("object")]
        public MapObjectPosition[] Entries { get; set; }
    }
#nullable restore warnings
}