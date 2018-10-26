namespace TDS.Instance.Map {
    using System;
    using System.Collections.Generic;
    using GTANetworkAPI;
    using TDS.Enum;

    class Map {
        public MapSync SyncData;
		public Dictionary<int, List<Vector3>> TeamSpawns = new Dictionary<int, List<Vector3>> ();
		public Dictionary<int, List<Vector3>> TeamRots = new Dictionary<int, List<Vector3>> ();
		public List<Vector3> MapLimits = new List<Vector3> ();
		public Vector3 MapCenter;
		public List<Vector3> BombPlantPlaces = new List<Vector3> ();
	}

    [Serializable]
    class MapSync {
        public string Name = "unknown";
        public EMapType Type = EMapType.Normal;
        public Dictionary<ELanguage, string> Description = new Dictionary<ELanguage, string> {
            {
                ELanguage.English, "No info available."
            }, {
                ELanguage.German, "Keine Info verfügbar."
            }
        };
    }

    [Serializable]
    class CreatedMap {
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
    class Description {
        public string English;
        public string German;
    }

    [Serializable]
    class Position {
        public float X;
        public float Y;
        public float Z;
    }

    [Serializable]
    class TeamSpawn {
        public int Team;
        public float X;
        public float Y;
        public float Z;
        public float Rot;
    }

}
