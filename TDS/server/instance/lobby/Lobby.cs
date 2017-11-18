namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using System.Linq;
	using damagesys;
	using extend;
	using GTANetworkAPI;
	using player;

	partial class Lobby : Script {

		private static readonly Dictionary<string, Lobby> lobbysbyname = new Dictionary<string, Lobby> ();
		private static readonly Dictionary<int, Lobby> lobbysbyindex = new Dictionary<int, Lobby> ();

		internal string Name;
		internal int ID;
		internal bool IsPlayable = true;
		internal bool DeleteWhenEmpty = true;
		internal bool IsOfficial = false;
		private readonly bool playersInOwnDimension;
		internal bool IsMapCreateLobby = false;

		public Lobby () {
			API.OnPlayerDisconnected += OnPlayerDisconnected;
			API.OnClientEventTrigger += OnClientEventTrigger;
			API.OnPlayerRespawn += OnPlayerRespawn;
			API.OnPlayerWeaponSwitch += OnPlayerWeaponSwitch;
			API.OnEntityEnterColShape += OnEntityEnterColShape;
		}

		internal Lobby ( string name, int id = -1, bool gotRounds = true, bool isPlayable = true,
						bool playersInOwnDimension = false ) {
			this.Name = name;
			if ( id == -1 ) {
				int theID = 0;
				while ( lobbysbyindex.ContainsKey ( theID ) )
					theID++;
				this.ID = theID;
			} else {
				this.ID = id;
			}
			if ( !playersInOwnDimension ) {
				uint dim = 1;
				while ( dimensionsUsed.ContainsKey ( dim ) )
					dim++;
				this.dimension = dim;
				dimensionsUsed[dim] = this;
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
			foreach ( List<Client> playerlist in this.Players ) {
				for ( int j = playerlist.Count - 1; j >= 0; j-- ) {
					if ( !playerlist[j].Exists || playerlist[j].GetChar ( ).Lobby != this ) {
						playerlist.RemoveAt ( j );
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
