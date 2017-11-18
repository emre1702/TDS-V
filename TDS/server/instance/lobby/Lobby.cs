namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using System.Linq;
	using damagesys;
	using extend;
	using GTANetworkAPI;
	using player;

	partial class Lobby : Script {

		private static Dictionary<string, Lobby> lobbysbyname = new Dictionary<string, Lobby> ();
		private static Dictionary<int, Lobby> lobbysbyindex = new Dictionary<int, Lobby> ();

		internal string Name;
		internal int ID;
		internal bool IsPlayable = true;
		internal bool DeleteWhenEmpty = true;
		internal bool IsOfficial = false;
		private bool playersInOwnDimension;
		internal bool IsMapCreateLobby = false;

		public Lobby () {
			API.OnPlayerDisconnected += OnPlayerDisconnected;
			API.OnClientEventTrigger += OnClientEventTrigger;
			API.OnPlayerRespawn += OnPlayerRespawn;
			API.OnPlayerWeaponSwitch += OnPlayerWeaponSwitch;
			API.OnEntityEnterColShape += OnEntityEnterColShape;
		}

		internal Lobby ( string name, int ID = -1, bool gotRounds = true, bool isPlayable = true,
						bool playersInOwnDimension = false ) {
			this.Name = name;
			if ( ID == -1 ) {
				int theID = 0;
				while ( lobbysbyindex.ContainsKey ( theID ) )
					theID++;
				this.ID = theID;
			} else {
				this.ID = ID;
			}
			if ( !playersInOwnDimension ) {
				uint dimension = 1;
				while ( dimensionsUsed.ContainsKey ( dimension ) )
					dimension++;
				this.dimension = dimension;
				dimensionsUsed[dimension] = this;
			}
			lobbysbyname[name] = this;
			lobbysbyindex[this.ID] = this;
			this.GotRounds = gotRounds;
			this.IsPlayable = isPlayable;
			this.playersInOwnDimension = playersInOwnDimension;
			this.DmgSys = new Damagesys ( this );
		}

		private void Remove () {
			if ( this.playersInOwnDimension ) {
				foreach ( KeyValuePair<uint, Lobby> item in dimensionsUsed.Where ( entry => entry.Value == this ).
					ToList () ) {
					dimensionsUsed.Remove ( item.Key );
				}
			} else {
				dimensionsUsed.Remove ( this.dimension );
			}
			lobbysbyname.Remove ( this.Name );
			lobbysbyindex.Remove ( this.ID );
			this.roundEndTimer.Kill ();
			this.roundStartTimer.Kill ();
			this.countdownTimer.Kill ();

			if ( this.currentMap != null && this.currentMap.Type == "bomb" )
				this.StopRoundBomb ();

			this.FuncIterateAllPlayers ( ( player, teamID ) => { this.RemovePlayer ( player ); } );
		}

		private void RefreshPlayerList () {
			for ( int i = 0; i < this.Players.Count; i++ ) {
				for ( int j = this.Players[i].Count - 1; j >= 0; j-- ) {
					if ( !this.Players[i][j].Exists || this.Players[i][j].GetChar ( ).Lobby != this ) {
						this.Players[i].RemoveAt ( j );
					}
				}
			}
		}

		private static void OnEntityEnterColShape ( ColShape shape, NetHandle entity ) {
			Client player = API.Shared.GetPlayerFromHandle ( entity );
			if ( player != null ) {
				Character character = player.GetChar ();
				if ( lobbyBombTakeCol.ContainsKey ( character.Lobby ) ) {
					if ( character.Lifes > 0 && character.Team == terroristTeamID ) {
						character.Lobby.TakeBomb ( player );
					}
				}
			}
		}
	}

}
