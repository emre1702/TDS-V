namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using GTANetworkAPI;
	using map;
    using Newtonsoft.Json;

	partial class Arena {

		private static readonly Dictionary<uint, Lobby> sDimensionsUsed = new Dictionary<uint, Lobby> ();
		private List<Map> maps;
        private string mapsJson;      

		private readonly Dictionary<uint, uint> spawnCounter = new Dictionary<uint, uint> ();
		private Map currentMap;
		private List<Blip> mapBlips = new List<Blip> ();
		private Vector3 spawnpoint = new Vector3 ( 0, 0, 1000 ),
						spawnrotation;

		public void SetMapList ( List<Map> themaps, List<MapSync> themapssync ) {
            maps = themaps;
            mapsJson = JsonConvert.SerializeObject ( themapssync );
		}

		public Map GetRandomMap () {
            Map nextmap = maps[manager.utility.Utility.Rnd.Next ( 0, maps.Count )];
            return nextmap;
		}

		private Vector3[] GetMapRandomSpawnData ( uint teamID ) {
			Vector3[] list = new Vector3[2];
			spawnCounter[teamID]++;
			uint index = spawnCounter[teamID];
			if ( index >= currentMap.TeamSpawns[teamID].Count ) {
				index = 0;
				spawnCounter[teamID] = 0;
			}
			list[0] = currentMap.TeamSpawns[teamID][(int)index];
			list[1] = currentMap.TeamRots[teamID][(int)index];
			return list;
		}

		private void CreateTeamSpawnBlips () {
			foreach ( KeyValuePair<uint, List<Vector3>> entry in currentMap.TeamSpawns ) {
                Blip blip = NAPI.Blip.CreateBlip ( pos: entry.Value[0], dimension: Dimension );
                blip.Sprite = 491;
                blip.Color = teamBlipColors[(int)entry.Key];
                blip.Name = "Spawn " + Teams[(int)entry.Key];
                mapBlips.Add ( blip );
            }
        }

		private void CreateMapLimitBlips () {
            NAPI.Util.ConsoleOutput ( "create map limit blips" );
			foreach ( Vector3 maplimit in currentMap.MapLimits ) {
                NAPI.Util.ConsoleOutput ( "map limit add" );
                Blip blip = NAPI.Blip.CreateBlip ( maplimit, Dimension );
				blip.Sprite = 441;
				blip.Name = "Limit";
				mapBlips.Add ( blip );
			}
		}

		private void DeleteMapBlips () {
			foreach ( Blip blip in mapBlips ) {
				blip.Delete ();
			}
			mapBlips = new List<Blip> ();
		}

		public void AddSpawnPoint ( Vector3 point, Vector3 rotation ) {
			spawnpoint = point;
			spawnrotation = rotation;
		}
	}

}
