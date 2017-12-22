namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using System.Threading.Tasks;
	using GTANetworkAPI;
	using map;
    using TDS.server.enums;
    using utility;

	partial class Arena {

		private static readonly Dictionary<uint, Lobby> sDimensionsUsed = new Dictionary<uint, Lobby> ();
		private Queue<string> mapNames;
		private Dictionary<Language, List<string>> mapDescriptions;

		private readonly Dictionary<uint, uint> spawnCounter = new Dictionary<uint, uint> ();
		private uint dimension;
		private Map currentMap;
		private List<Blip> mapBlips = new List<Blip> ();
		private Vector3 spawnpoint = new Vector3 ( 0, 0, 1000 ),
						spawnrotation;

		public void AddMapList ( List<string> newmapnames ) {
			mapNames = new Queue<string> ( newmapnames );
		}

		public void AddMapDescriptions ( Dictionary<Language, List<string>> mapdescriptions ) {
			mapDescriptions = new Dictionary<Language, List<string>> ( mapdescriptions );
		}

		public async Task<Map> GetRandomMap () {
			string mapname = mapNames.Dequeue ();
			mapNames.Enqueue ( mapname );
			return await manager.map.Map.GetMapClass ( mapname, this ).ConfigureAwait ( false );
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
				Blip blip = NAPI.Blip.CreateBlip ( entry.Value[0], dimension );
				blip.Sprite = 491;
				blip.Color = teamBlipColors[(int)entry.Key];
				blip.Name = "Spawn " + Teams[(int)entry.Key];
				mapBlips.Add ( blip );
			}
		}

		private void CreateMapLimitBlips () {
			foreach ( Vector3 maplimit in currentMap.MapLimits ) {
				Blip blip = NAPI.Blip.CreateBlip ( maplimit, dimension );
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
