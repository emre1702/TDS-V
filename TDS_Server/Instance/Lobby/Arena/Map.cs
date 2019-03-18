using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto;
using TDS_Server.Entity;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{

    partial class Arena
    {

        private MapDto currentMap;
        private List<MapDto> maps;
        private List<Blip> mapBlips = new List<Blip>();
        private string mapsJson;

        private MapDto GetNextMap()
        {
            MapDto map = GetVotedMap();
            if (map != null)
                return map;
            return GetRandomMap();
        }

        public MapDto GetRandomMap()
        {
            MapDto nextmap = maps[CommonUtils.Rnd.Next(0, maps.Count)];
            return nextmap;
        }

        private void CreateTeamSpawnBlips(MapDto map)
        {
            int i = 0;
            foreach (List<PositionRotationDto> entry in map.TeamSpawns)
            {
                Blip blip = NAPI.Blip.CreateBlip(pos: entry[0].Position, dimension: Dimension);
                blip.Sprite = 491;
                Team team = Teams[++i];
                blip.Color = team.Entity.BlipColor;
                blip.Name = "Spawn " + team.Entity.Name;
                mapBlips.Add(blip);
            }
        }

        private void CreateMapLimitBlips(MapDto map)
        {
            foreach (Vector3 maplimit in map.MapLimits)
            {
                Blip blip = NAPI.Blip.CreateBlip(maplimit, Dimension);
                blip.Sprite = 441;
                blip.Name = "Limit";
                mapBlips.Add(blip);
            }
        }

        private PositionRotationDto GetMapRandomSpawnData(Team team)
        {
            int index = team.SpawnCounter++;
            if (index >= currentMap.TeamSpawns[team.Entity.Index - 1].Count)
            {
                index = 0;
                team.SpawnCounter = 0;
            }
            return currentMap.TeamSpawns[team.Entity.Index - 1][index];
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

        public void SetMapList(List<MapDto> themaps, string syncjson = null)
        {
            maps = themaps;
            mapsJson = syncjson != null ? syncjson : JsonConvert.SerializeObject(themaps.Select(m => m.SyncedData).ToList());
        }
    }
}
