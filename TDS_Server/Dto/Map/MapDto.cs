using System.Collections.Generic;
using System.Xml.Serialization;
using TDS_Common.Dto;
using TDS_Common.Enum;
using EMapType = TDS_Server.Enums.EMapType;
using TDS_Server_DB.Entity.Player;
using System.Linq;
using TDS_Common.Dto.Map.Creator;
using TDS_Common.Manager.Utility;
using TDS_Common.Dto.Map;
using TDS_Server.Manager.Helper;

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

        [XmlElement("vehicles")]
        public MapVehiclesListDto Vehicles { get; set; }

        [XmlElement("bomb")]
        public MapBombInfoDto? BombInfo { get; set; }

        [XmlElement("target")]
        public Position3DDto? Target { get; set; }

        [XmlIgnore]
        public BrowserSyncedMapDataDto BrowserSyncedData { get; set; } = new BrowserSyncedMapDataDto();

        [XmlIgnore]
        public string ClientSyncedDataJson { get; set; }

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

            Vehicles = new MapVehiclesListDto
            {
                Entries = data.Vehicles.Select(o => new MapObjectPosition(o)).ToArray()
            };

            Target = data.Target != null && data.Type == TDS_Common.Enum.EMapType.Gangwar ? new Position3DDto(data.Target) : null;

            if (data.BombPlaces != null)
            {
                BombInfo = new MapBombInfoDto
                {
                    PlantPositions = data.BombPlaces.Select(pos => new Position3DDto(pos)).ToArray(),
                };
                BombInfo.PlantPositionsJson = Serializer.ToClient(BombInfo.PlantPositions);
            }

            LoadMapObjectsDataDto();
        }

        public void LoadMapObjectsDataDto()
        {
            var clientSyncedDataDto = new ClientSyncedDataDto
            {
                Name = Info.Name,
                BombPlaces = BombInfo?.PlantPositions?.Select(e => e.SwitchNamespace()).ToList(),
                MapEdges = LimitInfo?.Edges?.Select(e => e.SwitchNamespace()).ToList(),
                Objects = Objects?.Entries?.Select(e => e.ToMapCreatorPosition(0)).ToList(),
                Target = Target?.SwitchNamespace(),
                Vehicles = Vehicles?.Entries?.Select(e => e.ToMapCreatorPosition(0)).ToList(),
                Center = Target == null ? LimitInfo?.Center?.SwitchNamespace() : null
            };
            ClientSyncedDataJson = Serializer.ToClient(clientSyncedDataDto);
        }

        public static bool operator == (MapDto? thisMap, MapDto? otherMap) 
        {
            if (thisMap is null || otherMap is null)
                return ReferenceEquals(thisMap, otherMap);
            return thisMap.BrowserSyncedData.Id == otherMap.BrowserSyncedData.Id;
        }

        public static bool operator != (MapDto? thisMap, MapDto? otherMap)
        {
            if(thisMap is null || otherMap is null)
                return !ReferenceEquals(thisMap, otherMap);

            return thisMap.BrowserSyncedData.Id != otherMap.BrowserSyncedData.Id;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null)
                return false;

            if (!(obj is MapDto otherMap))
                return false;

            return BrowserSyncedData.Id == otherMap.BrowserSyncedData.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

#pragma warning restore CS8618 // Non-nullable field is uninitialized.
}
