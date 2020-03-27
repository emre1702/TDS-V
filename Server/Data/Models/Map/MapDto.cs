using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Models.Map.Creator;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.Map;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Manager.Utility;
using MapType = TDS_Server.Data.Enums.MapType;
using Position3DDto = TDS_Server.Data.Models.Map.Creator.Position3DDto;

namespace TDS_Server.Data.Models.Map
{
    #nullable enable
    #nullable disable warnings
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
        public bool IsBomb => Info.Type == MapType.Bomb;
        [XmlIgnore]
        public bool IsSniper => Info.Type == MapType.Sniper;

        private readonly Serializer _serializer;

        public MapDto() : this(new Serializer()) { }

        public MapDto(Serializer serializer) 
        {
            _serializer = serializer;    
        }

        public MapDto(MapCreateDataDto data, Serializer serializer) : this(serializer)
        {
            Info = new MapInfoDto
            {
                Name = data.Name,
                MinPlayers = data.Settings.MinPlayers,
                MaxPlayers = data.Settings.MaxPlayers,
                Type = (MapType)(int)data.Type
            };

            Descriptions = new MapDescriptionsDto
            {
                English = data.Description[(int)Language.English],
                German = data.Description[(int)Language.German]
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
                EdgesJson = serializer.ToClient(data.MapEdges)
            };

            Objects = new MapObjectsListDto
            {
                Entries = data.Objects.Select(o => new MapObjectPosition(o)).ToArray()
            };

            Vehicles = new MapVehiclesListDto
            {
                Entries = data.Vehicles.Select(o => new MapObjectPosition(o)).ToArray()
            };

            Target = data.Target != null && data.Type == TDS_Shared.Data.Enums.MapType.Gangwar ? new Position3DDto(data.Target) : null;

            if (data.BombPlaces != null)
            {
                BombInfo = new MapBombInfoDto
                {
                    PlantPositions = data.BombPlaces.Select(pos => new Position3DDto(pos)).ToArray(),
                };
                BombInfo.PlantPositionsJson = serializer.ToClient(BombInfo.PlantPositions);
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
                Center = Target is null ? LimitInfo?.Center?.SwitchNamespace() : null
            };
            ClientSyncedDataJson = _serializer.ToClient(clientSyncedDataDto);
        }

        public static bool operator ==(MapDto? thisMap, MapDto? otherMap)
        {
            if (thisMap is null || otherMap is null)
                return ReferenceEquals(thisMap, otherMap);
            return thisMap.BrowserSyncedData.Id == otherMap.BrowserSyncedData.Id;
        }

        public static bool operator !=(MapDto thisMap, MapDto otherMap)
        {
            if (thisMap is null || otherMap is null)
                return !ReferenceEquals(thisMap, otherMap);

            return thisMap.BrowserSyncedData.Id != otherMap.BrowserSyncedData.Id;
        }

        public override bool Equals(object obj)
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
}
