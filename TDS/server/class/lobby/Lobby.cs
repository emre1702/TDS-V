using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace Class { 
	partial class Lobby : Script {

		private static readonly System.Random rnd = new System.Random ();
		private static Dictionary<string, Lobby> lobbysbyname = new Dictionary<string, Lobby> ();
		private static Dictionary<int, Lobby> lobbysbyindex = new Dictionary<int, Lobby> ();

		public string name;
		public int id;
		public bool isPlayable = true;
		public bool deleteWhenEmpty = true;

		public Lobby ( ) {
			API.onPlayerDisconnected += this.OnPlayerDisconnected;
			API.onClientEventTrigger += this.OnClientEventTrigger;
			API.onPlayerRespawn += this.OnPlayerRespawn;
		}

		public Lobby ( string name, int ID = -1, bool gotRounds = true, bool isPlayable = true ) {
			this.name = name;
			if ( ID == -1 ) {
				int theID = 0;
				while ( lobbysbyindex.ContainsKey ( theID ) )
					theID++;
				this.id = theID;
			} else {
				this.id = ID;
			}
			int dimension = 1;
			while ( dimensionused.ContainsKey ( dimension ) )
				dimension++;
			this.dimension = dimension;
			dimensionused[dimension] = this;
			lobbysbyname[name] = this;
			lobbysbyindex [this.id] = this;
			this.gotRounds = gotRounds;
			this.isPlayable = isPlayable;
			this.damageSys = new Damage ( true );
		}

		public static Lobby GetLobbyByName ( string name ) {
			return lobbysbyname[name];
		}

		private void Remove ( ) {
			dimensionused.Remove ( this.dimension );
			lobbysbyname.Remove ( this.name );
			lobbysbyindex.Remove ( this.id );
			this.roundEndTimer.Kill ();
			this.roundStartTimer.Kill ();
			this.countdownTimer.Kill ();

			for ( int i = 0; i < this.players.Count; i++ ) {
				for ( int j = this.players[i].Count; j >= 0; j-- ) {
					Client player = this.players[i][j];
					this.RemovePlayer ( player );
				}
			}
		}
	}
}


