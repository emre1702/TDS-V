using System.Xml.Serialization;

namespace TDS_Common.Dto.Map
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
        public SyncedMapDataDto SyncedData => new SyncedMapDataDto();

        [XmlIgnore]
        public bool IsBomb => Info.Type == Enum.EMapType.Bomb;


    }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.

}
