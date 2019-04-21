﻿using System.Xml.Serialization;

namespace TDS_Common.Dto.Map
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
    public class MapTeamSpawnsDto
    {
        [XmlArray("spawn")]
        public MapPositionDto[] Spawns { get; set; }
    }
#pragma warning enable CS8618 // Non-nullable field is uninitialized.
}
