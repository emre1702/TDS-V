using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace Class {
	class Map {
		public string name;
		public string type = "arena";
		public Dictionary<string, string> description = new Dictionary<string, string> {
			{ "english", "No info available!" },
			{ "german", "Keine Info verfügbar!" }
		};
		public Dictionary<int, List<Vector3>> teamSpawns = new Dictionary<int, List<Vector3>> ();
		public Dictionary<int, List<Vector3>> teamRots = new Dictionary<int, List<Vector3>> ();
		public List<Vector3> mapLimits = new List<Vector3> ();
	}
}