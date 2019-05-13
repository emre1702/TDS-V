using System.Collections.Generic;
using System.Xml.Serialization;
using TDS_Common.Dto;
using TDS_Server.Enum;
using TDS_Server_DB.Entity;

namespace TDS_Server.Dto.Map
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized.

    [XmlRoot("TDSMap")]
    public class MapDto
    {
        [XmlElement("map")]
        public MapInfoDto Info { get; set; }

        [XmlElement("description")]
        public MapDescriptionsDto? Descriptions { get; set; }

        [XmlElement("teamspawns")]
        public MapTeamSpawnsListDto TeamSpawnsList { get; set; }

        [XmlElement("limit")]
        public MapLimitInfoDto LimitInfo { get; set; }

        [XmlElement("bomb")]
        public MapBombInfoDto? BombInfo { get; set; }

        [XmlIgnore]
        public SyncedMapDataDto SyncedData { get; set; } = new SyncedMapDataDto();

        [XmlIgnore]
        public List<PlayerMapRatings> Ratings { get; set; } = new List<PlayerMapRatings>();

        [XmlIgnore]
        public double RatingAverage { get; set; }

        [XmlIgnore]
        public bool IsBomb => Info.Type == EMapType.Bomb;
    }

#pragma warning restore CS8618 // Non-nullable field is uninitialized.
}