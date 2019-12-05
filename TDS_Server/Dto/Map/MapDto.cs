using System.Collections.Generic;
using System.Xml.Serialization;
using TDS_Common.Dto;
using TDS_Common.Enum;
using EMapType = TDS_Server.Enum.EMapType;
using TDS_Server_DB.Entity.Player;
using TDS_Common.Dto.Map;
using System.Linq;
using TDS_Common.Dto.Map.Creator;
using TDS_Server.Manager.Utility;
using TDS_Common.Manager.Utility;

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

        [XmlElement("objects")]
        public MapObjectsListDto Objects { get; set; }

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
        [XmlIgnore]
        public bool IsSniper => Info.Type == EMapType.Sniper;

        public MapDto() { }

        public MapDto(MapCreateDataDto data)
        {
            Info = new MapInfoDto
            {
                Name = data.Name,
                MinPlayers = data.Settings.MinPlayers,
                MaxPlayers = data.Settings.MaxPlayers,
                Type = (EMapType)(int)data.Type
            };

            Descriptions = new MapDescriptionsDto
            {
                English = data.Description[(int)ELanguage.English],
                German = data.Description[(int)ELanguage.German]
            };

            data.TeamSpawns.RemoveAll(l => l.Count == 0);
            TeamSpawnsList = new MapTeamSpawnsListDto { TeamSpawns = new MapTeamSpawnsDto[data.TeamSpawns.Count] };
            for (uint i = 0; i < data.TeamSpawns.Count; ++i)
            {
                TeamSpawnsList.TeamSpawns[i] = new MapTeamSpawnsDto { TeamID = i, Spawns = data.TeamSpawns[(int)i].Select(pos => new Position4DDto(pos)).ToArray() };
            }

            LimitInfo = new MapLimitInfoDto
            {
                Center = data.MapCenter != null ? new Position3DDto(data.MapCenter) : null,
                Edges = data.MapEdges.Select(pos => new Position3DDto(pos)).ToArray(),
                EdgesJson = Serializer.ToClient(data.MapEdges)
            };

            Objects = new MapObjectsListDto
            {
                Entries = data.Objects.Select(o => new MapObjectPosition(o)).ToArray()
            };

            if (data.BombPlaces != null)
                BombInfo = new MapBombInfoDto
                {
                    PlantPositions = data.BombPlaces.Select(pos => new Position3DDto(pos)).ToArray(),
                    PlantPositionsJson = Serializer.ToClient(data.BombPlaces)
                };
        }
    }

#pragma warning restore CS8618 // Non-nullable field is uninitialized.
}