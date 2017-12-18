namespace TDS.server.instance.map {

	using System.Collections.Generic;
	using GTANetworkAPI;
    using TDS.server.enums;

    class Map {
		public string Name = "unknown";
		public MapType Type = MapType.NORMAL;
		public Dictionary<Language, string> Description = new Dictionary<Language, string> {
			{
                Language.ENGLISH, "No info available."
			}, {
                Language.GERMAN, "Keine Info verfügbar."
			}
		};
		public Dictionary<uint, List<Vector3>> TeamSpawns = new Dictionary<uint, List<Vector3>> ();
		public Dictionary<uint, List<Vector3>> TeamRots = new Dictionary<uint, List<Vector3>> ();
		public List<Vector3> MapLimits = new List<Vector3> ();
		public Vector3 MapCenter;
		public List<Vector3> BombPlantPlaces = new List<Vector3> ();
	}

}
