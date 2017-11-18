namespace TDS.server.instance.map {

	using System.Collections.Generic;
	using GTANetworkAPI;

	class Map {
		public string Name = "unknown";
		public string Type = "normal";
		public Dictionary<string, string> Description = new Dictionary<string, string> {
			{
				"english", "No info available."
			}, {
				"german", "Keine Info verfügbar."
			}
		};
		public Dictionary<uint, List<Vector3>> TeamSpawns = new Dictionary<uint, List<Vector3>> ();
		public Dictionary<uint, List<Vector3>> TeamRots = new Dictionary<uint, List<Vector3>> ();
		public List<Vector3> MapLimits = new List<Vector3> ();
		public Vector3 MapCenter;
		public List<Vector3> BombPlantPlaces = new List<Vector3> ();
	}

}
