namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using System.Threading.Tasks;
	using GTANetworkAPI;
	using map;
	using utility;

	partial class Lobby {

		private static readonly Dictionary<uint, Lobby> dimensionsUsed = new Dictionary<uint, Lobby> ();
		private Queue<string> mapNames;
		private Dictionary<string, List<string>> mapDescriptions;

		private readonly Dictionary<uint, uint> spawnCounter = new Dictionary<uint, uint> ();
		private uint dimension;
		private Map currentMap;
		private Timer roundStartTimer,
						countdownTimer,
						roundEndTimer;
		private List<Blip> mapBlips = new List<Blip> ();
		private Vector3 spawnpoint = new Vector3 ( 0, 0, 1000 ),
						spawnrotation;

		public void AddMapList ( List<string> newmapnames ) {
			this.mapNames = new Queue<string> ( newmapnames );
		}

		public void AddMapDescriptions ( Dictionary<string, List<string>> mapdescriptions ) {
			this.mapDescriptions = new Dictionary<string, List<string>> ( mapdescriptions );
		}

		private uint GetDimension () {
			if ( this.playersInOwnDimension ) {
				uint dim = 1;
				while ( dimensionsUsed.ContainsKey ( dim ) )
					dim++;
				this.dimension = dim;
				dimensionsUsed[dim] = this;
				return dim;
			}
			return this.dimension;
		}

		public async Task<Map> GetRandomMap () {
			string mapname = this.mapNames.Dequeue ();
			this.mapNames.Enqueue ( mapname );
			return await manager.map.Map.GetMapClass ( mapname, this ).ConfigureAwait ( false );
		}

		private Vector3[] GetMapRandomSpawnData ( uint teamID ) {
			Vector3[] list = new Vector3[2];
			this.spawnCounter[teamID]++;
			uint index = this.spawnCounter[teamID];
			if ( index >= this.currentMap.TeamSpawns[teamID].Count ) {
				index = 0;
				this.spawnCounter[teamID] = 0;
			}
			list[0] = this.currentMap.TeamSpawns[teamID][(int)index];
			list[1] = this.currentMap.TeamRots[teamID][(int)index];
			return list;
		}

		private void CreateTeamSpawnBlips () {
			foreach ( KeyValuePair<uint, List<Vector3>> entry in this.currentMap.TeamSpawns ) {
				Blip blip = API.CreateBlip ( entry.Value[0], this.dimension );
				blip.Sprite = 491;
				blip.Color = this.teamBlipColors[(int)entry.Key];
				blip.Name = "Spawn " + this.Teams[(int)entry.Key];
				this.mapBlips.Add ( blip );
			}
		}

		private void CreateMapLimitBlips () {
			foreach ( Vector3 maplimit in this.currentMap.MapLimits ) {
				Blip blip = this.API.CreateBlip ( maplimit, this.dimension );
				blip.Sprite = 441;
				blip.Name = "Limit";
				this.mapBlips.Add ( blip );
			}
		}

		private void DeleteMapBlips () {
			foreach ( Blip blip in this.mapBlips ) {
				blip.Delete ();
			}
			this.mapBlips = new List<Blip> ();
		}

		public void AddSpawnPoint ( Vector3 point, Vector3 rotation ) {
			this.spawnpoint = point;
			this.spawnrotation = rotation;
		}
	}

}
