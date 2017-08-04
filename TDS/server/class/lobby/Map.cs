using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace Class {
	partial class Lobby : Script {

		private static Dictionary<int, Lobby> dimensionsUsed = new Dictionary<int, Lobby> ();
		private List<string> mapNames;
		private List<Dictionary<string, string>> mapDescriptions;

		private Dictionary<int, int> spawnCounter = new Dictionary<int, int> ();
		private int dimension;
		private Map currentMap;
		private Timer roundStartTimer;
		private Timer countdownTimer;
		private Timer roundEndTimer;
		private List<Blip> mapBlips = new List<Blip> ();

		public void AddMapList ( List<string> newmapnames, bool clone = true ) {
			if ( clone )
				this.mapNames = new List<string> ( newmapnames );
			else 
				this.mapNames = newmapnames;
		}

		public void AddMapDescriptions ( List<Dictionary<string, string>> mapdescriptions, bool clone = true ) {
			if ( clone )
				this.mapDescriptions = new List<Dictionary<string, string>> ( mapdescriptions );
			else
				this.mapDescriptions = mapdescriptions;
		}

		public Map GetRandomMap ( ) {
			int random = Manager.Utility.rnd.Next ( 0, this.mapNames.Count );
			return Manager.Map.GetMapClass ( this.mapNames[random], this );
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
	}
}


 