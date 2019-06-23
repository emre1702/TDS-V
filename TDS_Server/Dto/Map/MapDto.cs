using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;
using TDS_Common.Dto;
using TDS_Common.Dto.Map;
using TDS_Common.Enum;
using EMapType = TDS_Server.Enum.EMapType;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

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

        public MapDto() { }

        public MapDto(MapCreateDataDto data)
        {
            Info = new MapInfoDto
            {
                Name = data.Name,
                MinPlayers = data.MinPlayers,
                MaxPlayers = data.MaxPlayers,
                Type = data.Type
            };

            Descriptions = new MapDescriptionsDto
            {
                English = data.Description[ELanguage.English],
                German = data.Description[ELanguage.German]
            };

            TeamSpawnsList = new MapTeamSpawnsListDto { TeamSpawns = new MapTeamSpawnsDto[data.TeamSpawns.Length] };
            for (uint i = 0; i < data.TeamSpawns.Length; ++i)
            {
                TeamSpawnsList.TeamSpawns[i] = new MapTeamSpawnsDto { TeamID = i, Spawns = data.TeamSpawns[i] };
            }

            LimitInfo = new MapLimitInfoDto
            {
                Center = data.MapCenter,
                Edges = data.MapEdges,
                EdgesJson = JsonConvert.SerializeObject(data.MapEdges)
            };

            BombInfo = new MapBombInfoDto
            {
                PlantPositions = data.BombPlaces,
                PlantPositionsJson = JsonConvert.SerializeObject(data.BombPlaces)
            };
        }
    }

#pragma warning restore CS8618 // Non-nullable field is uninitialized.
}