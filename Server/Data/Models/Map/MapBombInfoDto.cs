﻿using System.Xml.Serialization;
using TDS_Server.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map
{

#nullable disable warnings
    public class MapBombInfoDto
    {
        [XmlElement("plantpos")]
        public Position3DDto[] PlantPositions { get; set; }

        [XmlIgnore]
        public string PlantPositionsJson { get; set; }
    }
#nullable restore warnings

}