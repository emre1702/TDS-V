using GTANetworkAPI;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Helper;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Utility;
using Object = GTANetworkAPI.Object;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Arena
    {
        private MapDto? _currentMap;
        private List<MapDto> _maps = new List<MapDto>();
        private List<Blip> _mapBlips = new List<Blip>();
        private List<Object> _mapObjects = new List<Object>();
        private string _mapsJson = string.Empty;

        private MapDto? GetNextMap()
        {
            MapDto? map = GetVotedMap();
            if (map != null)
                return map;
            return GetRandomMap();
        }

        public MapDto? GetRandomMap()
        {
            if (_maps.Count == 0)
                return null;
            if (_maps.Count == 1)
                return _maps[0];

            if (IsOfficial && CommonUtils.Rnd.NextDouble() * 100 <= SettingsManager.ArenaNewMapProbabilityPercent)
            {
                var map = MapCreator.GetRandomNewMap();
                if (map != null)
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
            var sumRatings = (int) Math.Floor(list.Sum(m => m.RatingAverage));
            var chooseAtRating = CommonUtils.Rnd.Next(sumRatings) + 1;
            double currentlyAtRating = 0;
            foreach (var map in list)
            {
                currentlyAtRating += map.RatingAverage;
                if (chooseAtRating <= currentlyAtRating)
                    return map;
            }

            // if I did a mistake, just return anything
            return list[CommonUtils.Rnd.Next(0, list.Count)];
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
                List<Vector3> regions = new List<Vector3>();
                foreach (var spawns in teamsSpawnList.Spawns)
                {
                    var position = spawns.ToVector3();
                    if (regions.Any(pos => pos.DistanceTo2D(position) < 5))
                        continue;
                    regions.Add(position);

                    Blip blip = NAPI.Blip.CreateBlip(pos: position, dimension: Dimension);
                    blip.Sprite = Constants.TeamSpawnBlipSprite;
                    Team team = Teams[(int)teamsSpawnList.TeamID];
                    blip.Color = team.Entity.BlipColor;
                    blip.Name = "Spawn " + team.Entity.Name;
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
                Blip blip = NAPI.Blip.CreateBlip(edge.ToVector3(), Dimension);
                blip.Sprite = Constants.MapLimitBlipSprite;
                blip.Name = "Limit " + ++i;
                _mapBlips.Add(blip);
            }
        }

        private void CreateMapObjects(MapDto map)
        {
            if (map.Objects is null || map.Objects.Entries is null || map.Objects.Entries.Length == 0)
                return;

            foreach (var objData in map.Objects.Entries)
            {
                var pos = new Vector3(objData.X, objData.Y, objData.Z);
                var rot = new Vector3(objData.RotX, objData.RotY, objData.RotZ);
                var obj = NAPI.Object.CreateObject(NAPI.Util.GetHashKey(objData.Name), pos, rot, 255, Dimension);
                _mapObjects.Add(obj);
            }
        }

        private Position4DDto? GetMapRandomSpawnData(Team? team)
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
            foreach (Blip blip in _mapBlips)
            {
                blip.Delete();
            }
            _mapBlips.Clear();
        }

        private void DeleteMapObjects()
        {
            foreach (var obj in _mapObjects)
            {
                obj.Delete();
            }
            _mapObjects.Clear();
        }

        public void SetMapList(List<MapDto> themaps, string? syncjson = null)
        {
            // Only choose maps with team-amount same as this lobby got teams (without spectator)
            _maps = themaps.Where(m => m.TeamSpawnsList.TeamSpawns.Length == Teams.Count - 1).ToList();

            _mapsJson = syncjson ?? Serializer.ToBrowser(_maps.Select(m => m.SyncedData).ToList());
        }
    }
}
