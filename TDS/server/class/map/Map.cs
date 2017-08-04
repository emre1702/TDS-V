using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace Class {
	struct Map {
		public bool created;
		public string name;
		public string type;
		public Dictionary<string, string> description;
		public Dictionary<int, List<Vector3>> teamSpawns;
		public Dictionary<int, List<Vector3>> teamRots;
		public List<Vector3> mapLimits;
	}
}