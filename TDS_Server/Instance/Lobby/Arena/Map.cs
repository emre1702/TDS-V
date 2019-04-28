using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Dto.Map;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Entity;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Helper;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{

    partial class Arena
    {

        private MapDto? currentMap;
        private List<MapDto> maps = new List<MapDto>();
        private List<Blip> mapBlips = new List<Blip>();
        private string mapsJson = string.Empty;

        private MapDto GetNextMap()
        {
            MapDto? map = GetVotedMap();
            if (map != null)
                return map;
            return GetRandomMap();
        }

        public MapDto GetRandomMap()
        {
            var mapsConsideringPlayersAmount = maps
                .Where(m => m.Info.MinPlayers >= Players.Count && m.Info.MaxPlayers <= Players.Count)
                .ToArray();
            if (mapsConsideringPlayersAmount.Length > 0)
                return mapsConsideringPlayersAmount[CommonUtils.Rnd.Next(0, mapsConsideringPlayersAmount.Length)];

            return maps[CommonUtils.Rnd.Next(0, maps.Count)];
        }

        private void CreateTeamSpawnBlips(MapDto map)
        {
            int i = 0;
            foreach (var teamspawn in map.TeamSpawnsList.TeamSpawns)
            {
                Blip blip = NAPI.Blip.CreateBlip(pos: teamspawn.Spawns[0].ToVector3(), dimension: Dimension);
                blip.Sprite = 491;
                Team team = Teams[++i];
                blip.Color = team.Entity.BlipColor;
                blip.Name = "Spawn " + team.Entity.Name;
                mapBlips.Add(blip);
            }
        }

        private void CreateMapLimitBlips(MapDto map)
        {
            foreach (MapPositionDto edge in map.LimitInfo.Edges)
            {
                Blip blip = NAPI.Blip.CreateBlip(edge.ToVector3(), Dimension);
                blip.Sprite = 441;
                blip.Name = "Limit";
                mapBlips.Add(blip);
            }
        }

        private MapPositionDto? GetMapRandomSpawnData(Team? team)
        {
            if (currentMap == null)
                return null;
            if (team == null)
                return null;
            int index = team.SpawnCounter++;
            var teamSpawns = currentMap.TeamSpawnsList.TeamSpawns[team.Entity.Index - 1].Spawns;
            if (index >= teamSpawns.Length)
            {
                index = 0;
                team.SpawnCounter = 0;
            }
            return teamSpawns[index];
        }

        private void DeleteMapBlips()
        {
            NAPI.Task.Run(() =>
            {
                foreach (Blip blip in mapBlips)
                {
                    blip.Delete();
                }
                mapBlips = new List<Blip>();
            });
        }

        public void SetMapList(List<MapDto> themaps, string? syncjson = null)
        {
            maps = themaps;
            mapsJson = syncjson != null ? syncjson : JsonConvert.SerializeObject(themaps.Select(m => m.SyncedData).ToList());
        }
    }
}
