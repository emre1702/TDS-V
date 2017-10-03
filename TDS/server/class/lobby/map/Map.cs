using System.Collections.Generic;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace Class {
	partial class Lobby {

		private static Dictionary<int, Lobby> dimensionsUsed = new Dictionary<int, Lobby> ();
		private List<string> mapNames;
		private Dictionary<string, List<string>> mapDescriptions;

		private Dictionary<int, int> spawnCounter = new Dictionary<int, int> ();
		private int dimension;
		private Map currentMap;
		private Timer roundStartTimer;
		private Timer countdownTimer;
		private Timer roundEndTimer;
		private List<Blip> mapBlips = new List<Blip> ();
		private Vector3 spawnpoint = new Vector3 ( 0, 0, 1000 );
		private Vector3 spawnrotation;

		public void AddMapList ( List<string> newmapnames ) {
			this.mapNames = new List<string> ( newmapnames );
		}

		public void AddMapDescriptions ( Dictionary<string, List<string>> mapdescriptions ) {
			this.mapDescriptions = new Dictionary<string, List<string>> ( mapdescriptions );
		}

		private int GetDimension ( ) {
			if ( this.playersInOwnDimension ) {
				int dimension = 1;
				while ( dimensionsUsed.ContainsKey ( dimension ) )
					dimension++;
				this.dimension = dimension;
				dimensionsUsed[dimension] = this;
				return dimension;
			} else {
				return this.dimension;
			}
		}

		public async Task<Map> GetRandomMap ( ) {
			int random = Manager.Utility.rnd.Next ( 0, this.mapNames.Count );
			return await Manager.Map.GetMapClass ( this.mapNames[random], this ).ConfigureAwait ( false );
		}

		private Vector3[] GetMapRandomSpawnData ( int teamID ) {
			Vector3[] list = new Vector3[2];
			this.spawnCounter[teamID]++;
			int index = this.spawnCounter[teamID];
			if ( index >= this.currentMap.teamSpawns[teamID].Count ) {
				index = 0;
				this.spawnCounter[teamID] = 0;
			}
			list[0] = this.currentMap.teamSpawns[teamID][index];
			list[1] = this.currentMap.teamRots[teamID][index];
			return list;
		}

		private void CreateTeamSpawnBlips ( ) {
			foreach ( KeyValuePair<int, List<Vector3>> entry in this.currentMap.teamSpawns ) { 
				Blip blip = API.createBlip ( entry.Value[0], this.dimension );
				blip.sprite = 491;
				blip.color = this.teamBlipColors[entry.Key];
				blip.name = "Spawn " + this.teams[entry.Key];
				this.mapBlips.Add ( blip );
			}
		}

		private void CreateMapLimitBlips ( ) {
			for ( int i = 0; i < this.currentMap.mapLimits.Count; i++ ) {
				Blip blip = API.createBlip ( this.currentMap.mapLimits[i], this.dimension );
				blip.sprite = 441;
				blip.name = "Limit";
				this.mapBlips.Add ( blip );
			}
		}

		private void DeleteMapBlips ( ) {
			for ( int i = 0; i < this.mapBlips.Count; i++ ) {
				this.mapBlips[i].delete ();
			}
			this.mapBlips = new List<Blip> ();
		}

		public void AddSpawnPoint ( Vector3 point, Vector3 rotation ) {
			this.spawnpoint = point;
			this.spawnrotation = rotation;
		}
	}
}


 