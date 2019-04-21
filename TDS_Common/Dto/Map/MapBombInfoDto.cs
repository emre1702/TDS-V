using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TDS_Common.Dto.Map
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
    public class MapBombInfoDto
    {
        [XmlArray("plantpos")]
        public MapPositionDto[] PlantPositions { get; set; }

        [XmlIgnore]
        public string PlantPositionsJson { get; set; }
    }
#pragma warning enable CS8618 // Non-nullable field is uninitialized.
}
