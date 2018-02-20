namespace TDS.server.instance.map {
    using System;
    using System.Collections.Generic;
	using GTANetworkAPI;
    using TDS.server.enums;

    public class Map {
        public MapSync SyncData;
		public Dictionary<int, List<Vector3>> TeamSpawns = new Dictionary<int, List<Vector3>> ();
		public Dictionary<int, List<Vector3>> TeamRots = new Dictionary<int, List<Vector3>> ();
		public List<Vector3> MapLimits = new List<Vector3> ();
		public Vector3 MapCenter;
		public List<Vector3> BombPlantPlaces = new List<Vector3> ();
	}

    [Serializable]
    public class MapSync {
        public string Name = "unknown";
        public MapType Type = MapType.NORMAL;
        public Dictionary<Language, string> Description = new Dictionary<Language, string> {
            {
                Language.ENGLISH, "No info available."
            }, {
                Language.GERMAN, "Keine Info verfügbar."
            }
        };
    }

    [Serializable]
    public class CreatedMap {
        public string Name;
        public string Type;
        public Description Descriptions;
        public int MinPlayers;
        public int MaxPlayers;
        public Position[] BombPlaces;
        public TeamSpawn[] MapSpawns;
        public Position[] MapLimitPositions;
        public Position MapCenter;
    }

    [Serializable]
    public class Description {
        public string English;
        public string German;
    }


    [Serializable]
    public class Position {
        public float X;
        public float Y;
        public float Z;
    }

    [Serializable]
    public class TeamSpawn {
        public int Team;
        public float X;
        public float Y;
        public float Z;
        public float Rot;
    }

}
