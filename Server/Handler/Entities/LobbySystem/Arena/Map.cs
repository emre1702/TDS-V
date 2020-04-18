using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Models.Map;
using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Core;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Utility;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        private MapDto? _currentMap;
        private List<MapDto> _maps = new List<MapDto>();
        private readonly List<IBlip> _mapBlips = new List<IBlip>();
        private string _mapsJson = string.Empty;

        private MapDto? GetNextMap()
        {
            MapDto? map = GetVotedMap();
            if (map is { })
                return map;
            return GetRandomMap();
        }

        public MapDto? GetRandomMap()
        {
            if (_maps.Count == 0)
                return null;
            if (_maps.Count == 1)
                return _maps[0];

            if (IsOfficial && SharedUtils.Rnd.NextDouble() * 100 <= SettingsHandler.ServerSettings.ArenaNewMapProbabilityPercent)
            {
                var map = _mapsLoadingHandler.GetRandomNewMap();
                if (map is { })
                    return map;
            }

            var mapsConsideringPlayersAmount = _maps
                .Where(m => m.Info.MinPlayers >= Players.Count && m.Info.MaxPlayers <= Players.Count)
                .ToList();
            if (mapsConsideringPlayersAmount.Count > 0)
                return GetRandomMapFromList(mapsConsideringPlayersAmount);

            return GetRandomMapFromList(_maps);
        }

        private static MapDto GetRandomMapFromList(List<MapDto> list)
        {
            var sumRatings = (int)Math.Floor(list.Sum(m => m.RatingAverage));
            var chooseAtRating = SharedUtils.Rnd.Next(sumRatings) + 1;
            double currentlyAtRating = 0;
            foreach (var map in list)
            {
                currentlyAtRating += map.RatingAverage;
                if (chooseAtRating <= currentlyAtRating)
                    return map;
            }

            // if I did a mistake, just return anything
            return SharedUtils.GetRandom(list);
        }

        /// <summary>
        /// Creates a blip for every team-spawn "region".
        /// If many spawns are at one position/region, only one blip will get created.
        /// But if there are many spawn-regions, there will be multiple blips.
        /// </summary>
        /// <param name="map"></param>
        private void CreateTeamSpawnBlips(MapDto map)
        {
            foreach (var teamsSpawnList in map.TeamSpawnsList.TeamSpawns)
            {
                if (Teams.Count < teamsSpawnList.TeamID)
                    return;
                var regions = new List<Position3D>();
                foreach (var spawns in teamsSpawnList.Spawns)
                {
                    var position = spawns.To3D();
                    if (regions.Any(pos => pos.DistanceTo2D(position) < 5))
                        continue;
                    regions.Add(position);

                    ITeam team = Teams[(int)teamsSpawnList.TeamID];
                    IBlip blip = ModAPI.Blip.Create(SharedConstants.TeamSpawnBlipSprite, position, color: team.Entity.BlipColor,
                        name: "Spawn " + team.Entity.Name, dimension: Dimension);

                    _mapBlips.Add(blip);
                }
            }
        }

        private void CreateMapLimitBlips(MapDto map)
        {
            if (map.LimitInfo.Edges is null)
                return;
            int i = 0;
            foreach (Position3DDto edge in map.LimitInfo.Edges)
            {
                IBlip blip = ModAPI.Blip.Create(SharedConstants.MapLimitBlipSprite, edge, name: "Limit " + ++i, dimension: Dimension);
                _mapBlips.Add(blip);
            }
        }

        private Position4DDto? GetMapRandomSpawnData(ITeam? team)
        {
            if (_currentMap is null)
                return null;
            if (team is null)
                return null;
            int index = team.SpawnCounter++;
            var teamSpawns = _currentMap.TeamSpawnsList.TeamSpawns[team.Entity.Index - 1].Spawns;
            if (index >= teamSpawns.Length)
            {
                index = 0;
                team.SpawnCounter = 0;
            }
            return teamSpawns[index];
        }

        private void DeleteMapBlips()
        {
            foreach (IBlip blip in _mapBlips)
            {
                blip.Delete();
            }
            _mapBlips.Clear();
        }

        public void SetMapList(List<MapDto> themaps, string? syncjson = null)
        {
            // Only choose maps with team-amount same as this lobby got teams (without spectator)
            _maps = themaps.Where(m => m.TeamSpawnsList.TeamSpawns.Length == Teams.Count - 1).ToList();

            _mapsJson = syncjson ?? Serializer.ToBrowser(_maps.Select(m => m.BrowserSyncedData).ToList());
        }
    }
}
