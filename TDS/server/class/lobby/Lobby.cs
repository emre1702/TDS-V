using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;

namespace Class { 
	partial class Lobby {

		private static Dictionary<string, Lobby> lobbysbyname = new Dictionary<string, Lobby> ();
		private static Dictionary<int, Lobby> lobbysbyindex = new Dictionary<int, Lobby> ();

		public string name;
		public int id;
		public bool isPlayable = true;
		public bool deleteWhenEmpty = true;
		public bool isOfficial = false;

		public static void LobbyOnStart ( API api ) {
			api.onPlayerDisconnected += OnPlayerDisconnected;
			api.onClientEventTrigger += OnClientEventTrigger;
			api.onPlayerRespawn += OnPlayerRespawn;
			api.onPlayerWeaponSwitch += OnPlayerWeaponSwitch;
			api.onEntityEnterColShape += OnEntityEnterColShape;
		}

		public Lobby ( ) { }

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
			while ( dimensionsUsed.ContainsKey ( dimension ) )
				dimension++;
			this.dimension = dimension;
			dimensionsUsed[dimension] = this;
			lobbysbyname[name] = this;
			lobbysbyindex [this.id] = this;
			this.gotRounds = gotRounds;
			this.isPlayable = isPlayable;
			this.damageSys = new Damagesys ( true );
		}

		public static Lobby GetLobbyByName ( string name ) {
			return lobbysbyname[name];
		}

		private void Remove ( ) {
			dimensionsUsed.Remove ( this.dimension );
			lobbysbyname.Remove ( this.name );
			lobbysbyindex.Remove ( this.id );
			this.roundEndTimer.Kill ();
			this.roundStartTimer.Kill ();
			this.countdownTimer.Kill ();

			if ( this.currentMap != null && this.currentMap.type == "bomb" )
				this.StopRoundBomb ();

			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				this.RemovePlayer ( player );
			} );
		}

		private void RefreshPlayerList ( ) {
			for ( int i = 0; i < this.players.Count; i++ ) {
				for ( int j = this.players[i].Count - 1; j >= 0; j-- ) {
					if ( !this.players[i][j].exists || this.players[i][j].GetChar ().lobby != this ) {
						this.players[i].RemoveAt ( j );
					} 
				}
			}
		}

		private static void OnEntityEnterColShape ( ColShape shape, NetHandle entity ) {
			Client player = API.shared.getPlayerFromHandle ( entity );
			if ( player != null ) {
				Character character = player.GetChar ();
				if ( lobbyBombTakeCol.ContainsKey ( character.lobby ) ) {
					if ( character.lifes > 0 && character.team == 1 ) {
						character.lobby.TakeBomb ( player );
					}
				}
			}
		}
	}
}


