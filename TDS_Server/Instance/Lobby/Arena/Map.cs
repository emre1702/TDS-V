using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Dto;
using TDS_Server.Entity;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby {

	partial class Arena {

        private MapDto currentMap;
        private List<MapDto> maps;
        private List<Blip> mapBlips = new List<Blip>();
        private int[] spawnCounter;

        private MapDto GetNextMap()
        {
            MapDto map = GetVotedMap();
            if (map != null)
                return map;
            return GetRandomMap();
        }

        public MapDto GetRandomMap()
        {
            MapDto nextmap = maps[Utils.Rnd.Next(0, maps.Count)];
            return nextmap;
        }

        private void CreateTeamSpawnBlips(MapDto map)
        {
            int i = 0;
            foreach (List<PositionRotationDto> entry in map.TeamSpawns)
            {
                Blip blip = NAPI.Blip.CreateBlip(pos: entry[0].Position, dimension: Dimension);
                blip.Sprite = 491;
                Teams team = Teams[i++];
                blip.Color = team.BlipColor;
                blip.Name = "Spawn " + team.Name;
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

        private PositionRotationDto GetMapRandomSpawnData(Teams team)
        {
            int teamindex = (int)team.Index;
            int index = ++spawnCounter[teamindex-1];
            if (index >= currentMap.TeamSpawns[teamindex-1].Count)
            {
                index = 0;
                spawnCounter[teamindex-1] = 0;
            }
            return currentMap.TeamSpawns[teamindex-1][index];
        }

        private void DeleteMapBlips()
        {
            foreach (Blip blip in mapBlips)
            {
                blip.Delete();
            }
            mapBlips = new List<Blip>();
        }

        /*private static readonly Dictionary<uint, Lobby> sDimensionsUsed = new Dictionary<uint, Lobby> ();
		
        private string mapsJson;      

		private readonly Dictionary<int, uint> spawnCounter = new Dictionary<int, uint> ();
		
		


		public void SetMapList ( List<Map> themaps, List<MapSync> themapssync ) {
            maps = themaps;
            mapsJson = JsonConvert.SerializeObject ( themapssync );
		}

		*/
    }

}
