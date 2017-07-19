using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Server.Constant;
using System;

namespace Class { 
	class Lobby : Script {
		// Static //
		private static readonly Random rnd = new Random ();
		private static Dictionary<int, Lobby> dimensionused = new Dictionary<int, Lobby> ();
		private static Dictionary<string, Lobby> lobbysbyname = new Dictionary<string, Lobby> ();
		private static Dictionary<int, Lobby> lobbysbyindex = new Dictionary<int, Lobby> ();

		// Object //
		public string name;
		public int id;
		public bool isPlayable = true;
		public bool gotRounds = true;
		private int countdownTime = 3;
		private int roundTime = 4 * 60;
		public int roundEndTime = 8;
		private int armor = 100;
		private int health = 100;
		private int lifes = 1;
		private int dimension;
		private string status;
		private Dictionary<int, int> spawnCounter = new Dictionary<int, int> ();
		private bool mixTeamsAfterRound = true;
		private Class.Map currentMap;
		private Timer roundStartTimer;
		private Timer countdownTimer;
		private Timer roundEndTimer;
		public List<string> teams = new List<string> { "Spectator" };
		private List<PedHash> teamSkins = new List<PedHash> { (PedHash) ( 225514697 ) };
		private List<List<Client>> players = new List<List<Client>> { new List<Client>() };
		private List<List<Client>> alivePlayers = new List<List<Client>> { new List<Client> () };
		private List<WeaponHash> weapons = new List<WeaponHash> ();
		private List<int> weaponsAmmo = new List<int> ();
		public bool deleteWhenEmpty = true;
		public Damage damageSys;
		private List<Blip> roundBlips = new List<Blip> ();
		private Dictionary<Client, List<Client>> spectatingMe = new Dictionary<Client, List<Client>> ();


		public Lobby ( ) {
			API.onPlayerDeath += this.OnPlayerDeath;
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


		/// SETTER & ADDER & GETTER ///
		public void AddTeam ( string name, PedHash hash ) {
			this.teams.Add ( name );
			this.teamSkins.Add ( hash );
			this.players.Add ( new List<Client> () );
			this.spawnCounter[this.teamSkins.Count-1] = 0;
		}

		public void AddPlayer ( Client player, bool spectator = false ) {
			player.freeze ( true );
			Class.Character character = player.GetChar ();
			character.lobby = this;
			character.spectating = null;
			player.dimension = this.dimension;
			if ( this.isPlayable )
				player.triggerEvent ( "onClientPlayerJoinLobby", spectator, this.countdownTime, this.roundTime, ( this.currentMap != null ? this.currentMap.name : "unknown" ) );
			else { 
				player.position = new Vector3 ( rnd.Next ( -10, 10 ), rnd.Next ( -10, 10 ), 1000 );
				player.stopSpectating ();
				player.triggerEvent ( "onClientPlayerLeaveLobby" );
			}

			if ( this.currentMap != null && this.currentMap.mapLimits.Count > 0 ) {
				player.triggerEvent ( "sendClientMapData", this.currentMap.mapLimits );
			}
			if ( spectator ) {
				this.players[0].Add ( player );
				character.team = 0;
			} else {
				int teamID = this.GetTeamIDWithFewestMember ();
				this.players[teamID].Add ( player );
				player.setSkin ( this.teamSkins[teamID] );
				character.team = teamID;
				if ( this.countdownTimer != null && this.countdownTimer.isRunning ) {
					this.SetPlayerReadyForRound ( player, teamID );
				} else {
					int teamsinround = this.GetTeamAmountStillInRound ();
					API.consoleOutput ( teamsinround + " teams still in round" );
					if ( teamsinround < 2 ) {
						this.EndRoundEarlier ();
						API.consoleOutput ( "End round earlier because of joined player" );
					}  else
						this.RespawnPlayerInSpectateMode ( player );
				}
			}
		}

		public void AddWeapon ( WeaponHash weapon, int ammo ) {
			this.weapons.Add ( weapon );
			this.weaponsAmmo.Add ( ammo );
		}

		public void Start ( ) {
			if ( this.isPlayable )
				this.StartMapChoose ();
		}

		////////////////////////////////////////////////////////////////


		public void StartMapChoose ( ) {
			this.status = "mapchoose";
			API.consoleOutput ( this.status );

			this.currentMap = Manager.Map.GetRandomMap ();

			int tsindex = 1;
			while ( this.currentMap.teamSpawns.ContainsKey ( tsindex ) ) {
				Blip blip = API.createBlip ( this.currentMap.teamSpawns[tsindex][0], this.dimension );
				blip.sprite = 491;
				blip.name = "Spawn " + this.teams[tsindex];
				this.roundBlips.Add ( blip );
				tsindex++;
			}
			
			for ( int i = 0; i < this.currentMap.mapLimits.Count; i++ ) {
				Blip blip = API.createBlip ( this.currentMap.mapLimits[i], this.dimension );
				blip.sprite = 441;
				blip.name = "Limit";
				this.roundBlips.Add ( blip );
			}
			if ( this.mixTeamsAfterRound )
				this.MixTeams ();
			this.roundStartTimer = Timer.SetTimer ( this.StartRoundCountdown, this.roundEndTime * 1000 / 2, 1 );
			if ( this.currentMap.mapLimits.Count > 0 )
				this.SendAllPlayerEvent ( "sendClientMapData", 0, this.currentMap.mapLimits );
		}

		private void StartRoundCountdown ( ) {
			this.status = "countdown";
			API.consoleOutput ( this.status );
			this.spectatingMe = new Dictionary<Client, List<Client>> ();
			for ( int i = 0; i < this.players.Count; i++ )
				for ( int j = 0; j < this.players[i].Count; j++ ) {
					this.SetPlayerReadyForRound ( this.players[i][j], i );
					API.sendNativeToPlayer ( this.players[i][j], Hash.DO_SCREEN_FADE_IN, this.countdownTime * 1000 );
					this.players[i][j].triggerEvent ( "onClientCountdownStart", this.currentMap.name );
					if ( i == 0 )
						this.SpectateAllTeams ( this.players[i][j], true );
				}
			this.countdownTimer = Timer.SetTimer ( this.StartRound, this.countdownTime * 1000 + 200, 1 );
		}

		private void StartRound ( ) {
			this.status = "round";
			API.consoleOutput ( this.status );
			if ( this.gotRounds )
				this.roundEndTimer = Timer.SetTimer ( this.EndRound, this.roundTime * 1000, 1 );
			this.alivePlayers = new List<List<Client>> ();
			for ( int i = 0; i < this.players.Count; i++ ) {
				this.alivePlayers.Add ( new List<Client> () );
				for ( int j = 0; j < this.players[i].Count; j++ ) {
					Client player = this.players[i][j];
					Class.Character character = player.GetChar ( );
					player.triggerEvent ( "onClientRoundStart", i == 0, this.players[i] );
					character.team = i;
					if ( i != 0 ) {
						character.lifes = this.lifes;
						this.alivePlayers[i].Add ( player );
						player.freeze ( false );
						this.GivePlayerWeapons ( player );
					}
				}
			}
		}

		private void EndRound ( ) {
			this.status = "roundend";
			API.consoleOutput ( this.status );
			this.roundStartTimer.Kill ();
			for ( int i = 0; i < this.roundBlips.Count; i++ ) {
				this.roundBlips[i].delete ();
			}
			this.roundBlips = new List<Blip> ();
			bool foundone = false;
			API.sendNativeToPlayersInDimension ( this.dimension, Hash.DO_SCREEN_FADE_OUT, this.roundEndTime/2 * 1000 );
			for ( int i = 0; i < this.players.Count && !foundone; i++ ) {
				if ( this.players[i].Count > 0 )
					foundone = true;
			}
			if ( foundone ) { 
				this.roundStartTimer = Timer.SetTimer ( this.StartMapChoose, this.roundEndTime * 1000 / 2, 1 );
				this.SendAllPlayerEvent ( "onClientRoundEnd" );
			} else if ( this.deleteWhenEmpty ) {
				this.Remove ();
			}
		}

		public void EndRoundEarlier ( ) {
			if ( this.roundEndTimer != null )
				this.roundEndTimer.Kill ();
			if ( this.countdownTimer != null )
				this.countdownTimer.Kill ();
			this.EndRound ();
		}

		private void CheckLobbyForEnoughAlive ( ) {
			int teamsinround = this.GetTeamAmountStillInRound ();
			if ( teamsinround < 2 ) {
				this.EndRoundEarlier ();
			}
		}

		private void OnPlayerDeath ( Client player, NetHandle killer, int weapon ) {
			Class.Character character = player.GetChar ();
			if ( character.lifes > 0 ) {
				character.lifes--;
				if ( character.lifes == 0 ) {
					Lobby lobby = character.lobby;
					int teamID = character.team;
					int aliveindex = lobby.alivePlayers[teamID].IndexOf ( player );
					lobby.PlayerCantBeSpectatedAnymore ( player, aliveindex, teamID );
					lobby.alivePlayers[teamID].RemoveAt ( aliveindex );
					lobby.CheckLobbyForEnoughAlive ();
				}
			}
		}

		private void OnPlayerDisconnected ( Client player, string reason ) {
			Class.Character character = player.GetChar ();
			int teamID = character.team;
			Lobby lobby = character.lobby;
			if ( character.lifes > 0 ) {
				int aliveindex = lobby.alivePlayers[teamID].IndexOf ( player );
				lobby.PlayerCantBeSpectatedAnymore ( player, aliveindex, teamID );
				lobby.alivePlayers[teamID].RemoveAt ( aliveindex );
				lobby.CheckLobbyForEnoughAlive ();
			}
			lobby.players[teamID].Remove ( player );
		}

		private void SendAllPlayerEvent ( string eventName, int teamindex = 0, dynamic arg1 = null ) {
			if ( teamindex == 0 )
				for ( int i = 0; i < this.players.Count; i++ )
					for ( int j = 0; j < this.players[i].Count; j++ )
						if ( arg1 == null )
							this.players[i][j].triggerEvent ( eventName );
						else
							this.players[i][j].triggerEvent ( eventName, arg1 );
			else
				for ( int j = 0; j < this.players[teamindex].Count; j++ )
					if ( arg1 == null )
						this.players[teamindex][j].triggerEvent ( eventName );
					else
						this.players[teamindex][j].triggerEvent ( eventName, arg1 );
		}

		private int GetTeamIDWithFewestMember ( ) {
			int lastteamID = 1;
			int lastteamcount = this.players[1].Count;
			for ( int k = 2; k < this.players.Count; k++ ) {
				int count = this.players[k].Count;
				if ( count < lastteamcount || count == lastteamcount && rnd.Next ( 1, 2 ) == 1 ) {
					lastteamID = k;
					lastteamcount = count;
				}
			}
			return lastteamID;
		}

		private void MixTeams ( ) {
			List<List<Client>> newplayerslist = new List<List<Client>> { new List<Client>() };
			for ( int i = 0; i < this.players[0].Count; i++ ) {
				newplayerslist[0].Add ( this.players[0][i] );
			}
			for ( int i = 1; i < this.players.Count; i++ )
				newplayerslist.Add ( new List<Client> () );
			for ( int i = 1; i < this.players.Count; i++ ) {
				this.spawnCounter[i] = 0;
				for ( int j = 0; j < this.players[i].Count; j++ ) {
					int teamID = this.GetTeamIDWithFewestMember ();
					newplayerslist[teamID].Add ( players[i][j] );
					this.players[i][j].setSkin ( this.teamSkins[teamID] );
				}
			}
			this.players = new List<List<Client>> ( newplayerslist );

		}

		private Vector3[] GetMapRandomSpawnData ( int teamID ) {
			Vector3[] list = new Vector3[2];
			this.spawnCounter[teamID]++;
			int index = this.spawnCounter[teamID];
			if ( index >= this.currentMap.teamSpawns[teamID].Count ) {
				index = 0;
				this.spawnCounter[teamID] = 0;
			}
			list[0] = this.currentMap.teamSpawns[teamID][index];
			list[1] = this.currentMap.teamRots[teamID][index];
			return list;
		}

		private void SetPlayerReadyForRound ( Client player, int teamID ) {
			player.armor = this.armor;
			player.health = this.health;
			this.Spectate ( player, player );
			if ( teamID > 0 ) {
				Vector3[] spawndata = this.GetMapRandomSpawnData ( teamID );
				player.position = spawndata[0];
				player.rotation = spawndata[1];
			}
			player.freeze ( true );
			API.removeAllPlayerWeapons ( player );
		}

		private void GivePlayerWeapons ( Client player ) {
			for ( int i = 0; i < this.weapons.Count; i++ ) {
				API.givePlayerWeapon ( player, this.weapons[i], this.weaponsAmmo[i], false, true );
			}
		}

		/* public void sendNotifaction ( string message, int team = -1 ) {
			if ( team == -1 ) {
				for ( int i = 0; i < this.players.Count; i++ ) {
					for ( int j = 0; j < this.players[i].Count; j++ ) {
						API.sendNotificationToPlayer ( this.players[i][j], cLanguageManager.getLang ( this.players[i][j], message ) );
					}
				}
			} else {
				for ( int j = 0; j < this.players[team].Count; j++ ) {
					API.sendNotificationToPlayer ( this.players[team][j], cLanguageManager.getLang ( this.players[team][j], message ) );
				}
			}
		}*/


		//////////////////// SPECTATE ////////////////////
		private void SpectateTeammate ( Client player, bool forwards = true, int givenindex = -1, int giventeam = -1 ) {
			Class.Character character = player.GetChar ();
			Client spectating = character.spectating ?? player;
			int teamID = character.team;
			if ( this.alivePlayers.Count == 0 )
				this.SpectateAllTeams ( player, forwards, givenindex, giventeam );
			else {
				int index = this.alivePlayers[teamID].IndexOf ( spectating );
				if ( index == -1 )
					index = 0;
				if ( forwards ) {
					index++;
					if ( index >= this.alivePlayers.Count )
						index = 0;
				} else {
					index--;
					if ( index < 0 )
						index = this.alivePlayers.Count - 1;
				}
				this.Spectate ( player, this.alivePlayers[teamID][index] );
			}
		}

		private void SpectateAllTeams ( Client player, bool forwards = true, int givenindex = -1, int giventeam = -1 ) {
			Character character = player.GetChar ();
			Client spectating = character.spectating ?? player;
			int teamID = giventeam != -1 ? giventeam : spectating.GetChar ().team;
			if ( teamID == 0 )
				teamID = 1;
			int amountteams = this.alivePlayers.Count;
			int amounttried = 0;
			while ( this.alivePlayers[teamID].Count == 0 ) {
				teamID += forwards ? 1 : -1;
				amounttried++;
				givenindex = -1;
				if ( teamID == 0 )
					teamID = amountteams - 1;
				else if ( teamID == amountteams - 1 )
					teamID = 0;
				if ( amounttried == amountteams ) {
					this.Spectate ( player, player );
				}
			}
			int index = givenindex != -1 ? givenindex : this.alivePlayers[teamID].IndexOf ( spectating );
			if ( index == -1 )
				index = 0;
			if ( forwards ) {
				index++;
				if ( index >= this.alivePlayers.Count )
					index = 0;
			} else {
				index--;
				if ( index < 0 )
					index = this.alivePlayers.Count - 1;
			}
			this.Spectate ( player, this.alivePlayers[teamID][index] );
		}

		private void PlayerCantBeSpectatedAnymore ( Client player, int givenindex, int giventeam ) {
			if ( this.spectatingMe.ContainsKey ( player ) ) {
				for ( int i = 0; i < this.spectatingMe[player].Count; i++ ) {
					if ( this.spectatingMe[player][i].GetChar ().team == 0 )
						this.SpectateAllTeams ( this.spectatingMe[player][i], true, givenindex, giventeam );
					else
						this.SpectateTeammate ( this.spectatingMe[player][i], true, givenindex, giventeam );
				}
				this.spectatingMe.Remove ( player );
			}
		}

		private void Spectate ( Client player, Client target ) {
			Character character = player.GetChar ();
			if ( player != target ) {
				character.spectating = target;
				player.spectate ( target );
				if ( !this.spectatingMe.ContainsKey ( target ) ) {
					this.spectatingMe[target] = new List<Client> ();
				}
				this.spectatingMe[target].Add ( player );
			} else {
				if ( character.spectating != null ) {
					Client oldspectating = character.spectating;
					if ( this.spectatingMe.ContainsKey ( oldspectating ) ) {
						this.spectatingMe[oldspectating].Remove ( player );
					}
				}
				character.spectating = null;
				player.stopSpectating ();
			}
		}

		///////////////////////////////////////////////////////////////////////////////////////////////


		public static Lobby GetLobbyByName ( string name ) {
			return lobbysbyname[name];
		}

		public void RemovePlayer ( Client player ) {
			Class.Character character = player.GetChar ();
			int teamID = character.team;
			this.players[teamID].Remove ( player );
			int oldlifes = character.lifes;
			character.lifes = 0;
			if ( oldlifes > 0 ) {
				int aliveindex = this.alivePlayers[teamID].IndexOf ( player );
				this.PlayerCantBeSpectatedAnymore ( player, aliveindex, teamID );
				this.alivePlayers[teamID].Remove ( player );
				this.CheckLobbyForEnoughAlive ();
			}
			Manager.MainMenu.Join ( player );
		}

		private void RespawnPlayerInRound ( Client player ) {
			this.SetPlayerReadyForRound ( player, player.GetChar().team );
			player.freeze ( false );
		}

		private void RespawnPlayerInSpectateMode ( Client player ) {
			player.position = new Vector3 ( rnd.Next ( -10, 10 ), rnd.Next ( -10, 10 ), 1000 );
			player.freeze ( true );
			this.SpectateTeammate ( player );
			player.triggerEvent ( "onClientSpectateMode" );
		}

		private void OnPlayerRespawn ( Client player ) {
			Class.Character character = player.GetChar();
			Lobby lobby = character.lobby;
			if ( lobby.isPlayable ) {
				if ( character.lifes > 0 ) {
					lobby.RespawnPlayerInRound ( player );
				} else {
					lobby.RespawnPlayerInSpectateMode ( player );
				}
			} else {
				player.position = new Vector3 ( rnd.Next ( -10, 10 ), rnd.Next ( -10, 10 ), 1000 );
				player.freeze ( true );
			}
		}

		private void Remove ( ) {
			dimensionused.Remove ( this.dimension );
			lobbysbyname.Remove ( this.name );
			lobbysbyindex.Remove ( this.id );
			this.roundEndTimer.Kill ();
			this.roundStartTimer.Kill ();
			this.countdownTimer.Kill ();

			for ( int i=0; i < this.players.Count; i++ ) {
				for ( int j = this.players[i].Count; j >= 0; j-- ) {
					Client player = this.players[i][j];
					this.RemovePlayer ( player );
				}
			}
		}

		public void RespawnPlayerDeath ( Client player ) {
			// TO DO //
		}

		private void OnClientEventTrigger ( Client player, string eventName, params dynamic[] args ) {
			switch ( eventName ) {

				case "joinLobby":
					if ( lobbysbyindex.ContainsKey ( args[0] ) ) {
						Lobby lobby = lobbysbyindex[args[0]];
						lobby.AddPlayer ( player, args[1] );
					} else {
						/* player.sendNotification (  lobby doesn't exist ); */
						player.triggerEvent ( "onClientJoinMainMenu" );
					}
					break;

				case "spectateNext":
					Class.Character character = player.GetChar();
					if ( character.lifes == 0 && ( character.lobby.status == "round" || character.team == 0 && character.lobby.status == "countdown" ) ) {
						if ( character.team == 0 )
							character.lobby.SpectateAllTeams ( player, args[0] );
						else 
							character.lobby.SpectateTeammate ( player, args[0] );
					} 
					break;

				case "onPlayerWasTooLongOutsideMap":
					Class.Character character2 = player.GetChar ();
					if ( character2.lobby.isPlayable ) {
						character2.lobby.KillPlayer ( player, "too_long_outside_map" );
					}
					break;
			}
		}

		private int GetTeamAmountStillInRound ( int minalive = 1 ) {
			int amount = 0;
			for ( int i=1; i < this.alivePlayers.Count; i++ ) 
				if ( this.alivePlayers[i].Count >= minalive ) 
					amount++;
			return amount;
		}

		private void KillPlayer ( Client player, string reason ) {
			player.kill ();
			player.SendLangNotification ( reason );
		}

		public string GetTeamName ( int ID ) {
			return teams[ID];
		}


		/*private bool IsPointInPolygon ( Vector3 p ) {
			Map map = this.currentMap;
			if ( p.X < map.minX || p.X > map.maxX || p.Y < map.minY || p.Y > map.maxY ) {
				return false;
			}
			bool inside = false;
			for ( int i = 0, j = map.maplimit.Count - 1; i < map.maplimit.Count; j = i++ ) {
				if ( ( map.maplimit[i].Y > p.Y ) != ( map.maplimit[j].Y > p.Y ) &&
					p.X < ( map.maplimit[j].X - map.maplimit[i].X ) * ( p.Y - map.maplimit[i].Y ) / ( map.maplimit[j].Y - map.maplimit[i].Y ) + map.maplimit[i].X ) {
					inside = !inside;
				}
			}
			return inside;
		}*/

	}
}


