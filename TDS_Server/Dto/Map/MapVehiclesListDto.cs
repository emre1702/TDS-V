﻿using System.Xml.Serialization;

namespace TDS_Server.Dto.Map
{
#nullable disable warnings
    public class MapVehiclesListDto
    {
        [XmlElement("vehicle")]
        public MapObjectPosition[] Entries { get; set; }
    }
#nullable restore warnings
}