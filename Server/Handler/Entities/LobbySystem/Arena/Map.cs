﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
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
        #region Private Fields

        private readonly List<ITDSBlip> _mapBlips = new List<ITDSBlip>();
        private MapDto? _currentMap;
        private Position3D? _currentMapSpectatorPosition;
        private List<MapDto> _maps = new List<MapDto>();
        private string _mapsJson = string.Empty;

        #endregion Private Fields

        #region Public Methods

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

        public void SetMapList(List<MapDto> themaps, string? syncjson = null)
        {
            // Only choose maps with team-amount same as this lobby got teams (without spectator)
            _maps = themaps.Where(m => m.TeamSpawnsList.TeamSpawns.Length == Teams.Count - 1).ToList();

            _mapsJson = syncjson ?? Serializer.ToBrowser(_maps.Select(m => m.BrowserSyncedData).ToList());
        }

        #endregion Public Methods

        #region Private Methods

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

        private void CreateMapLimitBlips(MapDto map)
        {
            if (map.LimitInfo.Edges is null)
                return;
            int i = 0;
            foreach (Position3DDto edge in map.LimitInfo.Edges)
            {
                var blip = NAPI.Blip.CreateBlip(SharedConstants.MapLimitBlipSprite, edge.ToVector3(), 1f, 0, name: "Limit " + ++i, dimension: Dimension) as ITDSBlip;
                _mapBlips.Add(blip!);
            }
        }

        /// <summary>
        /// Creates a blip for every team-spawn "region". If many spawns are at one position/region,
        /// only one blip will get created. But if there are many spawn-regions, there will be
        /// multiple blips.
        /// </summary>
        /// <param name="map"></param>
        private void CreateTeamSpawnBlips(MapDto map)
        {
            foreach (var teamsSpawnList in map.TeamSpawnsList.TeamSpawns)
            {
                if (Teams.Count < teamsSpawnList.TeamID)
                    return;
                var regions = new List<Vector3>();
                foreach (var spawns in teamsSpawnList.Spawns)
                {
                    var position = spawns.ToVector3();
                    if (regions.Any(pos => pos.DistanceTo2D(position) < 5))
                        continue;
                    regions.Add(position);

                    var team = Teams[(int)teamsSpawnList.TeamID];
                    var blip = NAPI.Blip.CreateBlip(SharedConstants.TeamSpawnBlipSprite, position, 1f, team.Entity.BlipColor,
                        name: "Spawn " + team.Entity.Name, dimension: Dimension) as ITDSBlip;

                    _mapBlips.Add(blip!);
                }
            }
        }

        private void DeleteMapBlips()
        {
            foreach (ITDSBlip blip in _mapBlips)
            {
                blip.Delete();
            }
            _mapBlips.Clear();
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

        private MapDto? GetNextMap()
        {
            MapDto? map = GetVotedMap();
            if (map is { })
                return map;
            return GetRandomMap();
        }

        #endregion Private Methods
    }
}
