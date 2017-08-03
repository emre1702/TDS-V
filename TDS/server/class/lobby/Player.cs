using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Manager;

namespace Class {

	partial class Lobby : Script {

		public int armor = 100;
		public int health = 100;
		private int lifes = 1;
		private List<List<Client>> players = new List<List<Client>> { new List<Client> () };
		private List<List<Client>> alivePlayers = new List<List<Client>> { new List<Client> () };

		public void AddPlayer ( Client player, bool spectator = false ) {
			player.freeze ( true );
			Class.Character character = player.GetChar ();
			character.lobby = this;
			character.spectating = null;
			player.dimension = this.dimension;
			if ( this.isPlayable ) { 
				player.triggerEvent ( "onClientPlayerJoinLobby", spectator, this.countdownTime, this.roundTime, ( this.currentMap != null ? this.currentMap.name : "unknown" ) );
				this.SyncMapVotingOnJoin ( player );
			} else {
				player.position = new Vector3 ( Manager.Utility.rnd.Next ( -10, 10 ), Manager.Utility.rnd.Next ( -10, 10 ), 1000 );
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
				int teamID = this.GetTeamIDWithFewestMember ( this.players );
				this.players[teamID].Add ( player );
				player.setSkin ( this.teamSkins[teamID] );
				character.team = teamID;
				if ( this.countdownTimer != null && this.countdownTimer.isRunning ) {
					this.SetPlayerReadyForRound ( player, teamID );
				} else {
					int teamsinround = this.GetTeamAmountStillInRound ();
					API.shared.consoleOutput ( teamsinround + " teams still in round" );
					if ( teamsinround < 2 ) {
						this.EndRoundEarlier ();
						API.shared.consoleOutput ( "End round earlier because of joined player" );
					} else
						this.RespawnPlayerInSpectateMode ( player );
				}
			}
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
			API.shared.removeAllPlayerWeapons ( player );
			Damagesys.allHitters[player] = new Dictionary<Client, int> ();
			Damagesys.lastHitterDictionary.Remove ( player );
		}

		private void GivePlayerWeapons ( Client player ) {
			for ( int i = 0; i < this.weapons.Count; i++ ) {
				API.shared.givePlayerWeapon ( player, this.weapons[i], this.weaponsAmmo[i], false, true );
			}
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
				Damagesys.CheckLastHitter ( player, character );
				this.CheckLobbyForEnoughAlive ();
			}
			Damagesys.allHitters.Remove ( player );
			Manager.MainMenu.Join ( player );
		}

		private void RespawnPlayerInRound ( Client player ) {
			this.SetPlayerReadyForRound ( player, player.GetChar ().team );
			player.freeze ( false );
		}

		private void RespawnPlayerInSpectateMode ( Client player ) {
			player.position = new Vector3 ( Utility.rnd.Next ( -10, 10 ), Utility.rnd.Next ( -10, 10 ), 1000 );
			player.freeze ( true );
			this.SpectateTeammate ( player );
			player.triggerEvent ( "onClientSpectateMode" );
		}

		private static void OnPlayerRespawn ( Client player ) {
			Class.Character character = player.GetChar ();
			Lobby lobby = character.lobby;
			if ( lobby.isPlayable ) {
				if ( character.lifes > 0 ) {
					lobby.RespawnPlayerInRound ( player );
				} else {
					lobby.RespawnPlayerInSpectateMode ( player );
				}
			} else {
				player.position = new Vector3 ( Utility.rnd.Next ( -10, 10 ), Utility.rnd.Next ( -10, 10 ), 1000 );
				player.freeze ( true );
			}
		}

		public void OnPlayerDeath ( Client player, NetHandle killer, int weapon, Character character ) {
			if ( character.lifes > 0 ) {
				character.lifes--;
				if ( character.lifes == 0 ) {
					int teamID = character.team;
					int aliveindex = this.alivePlayers[teamID].IndexOf ( player );
					this.PlayerCantBeSpectatedAnymore ( player, aliveindex, teamID );
					this.alivePlayers[teamID].RemoveAt ( aliveindex );
					this.CheckLobbyForEnoughAlive ();
				}
			}
		}

		private static void OnPlayerDisconnected ( Client player, string reason ) {
			Character character = player.GetChar ();
			int teamID = character.team;
			Lobby lobby = character.lobby;
			if ( character.lifes > 0 ) {
				int aliveindex = lobby.alivePlayers[teamID].IndexOf ( player );
				lobby.PlayerCantBeSpectatedAnymore ( player, aliveindex, teamID );
				lobby.alivePlayers[teamID].RemoveAt ( aliveindex );
				Damagesys.CheckLastHitter ( player, character );
				lobby.CheckLobbyForEnoughAlive ();
			}
			lobby.players[teamID].Remove ( player );
		}

		private static void OnClientEventTrigger ( Client player, string eventName, params dynamic[] args ) {
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
					Class.Character character = player.GetChar ();
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

				case "onMapMenuOpen":
					player.GetChar ().lobby.SendMapsForVoting ( player );
					break;

				case "onMapVotingRequest":
					player.GetChar ().lobby.AddMapToVoting ( player, args[0] );
					break;
			}
		}

		public void SendAllPlayerEvent ( string eventName, int teamindex = -1, params object[] args ) {
			if ( teamindex == -1 )
				for ( int i = 0; i < this.players.Count; i++ )
					for ( int j = 0; j < this.players[i].Count; j++ )
						this.players[i][j].triggerEvent ( eventName, args );
			else
				for ( int j = 0; j < this.players[teamindex].Count; j++ )
					this.players[teamindex][j].triggerEvent ( eventName, args );
		}

		private void KillPlayer ( Client player, string reason ) {
			player.kill ();
			player.SendLangNotification ( reason );
		}

		[Command("leave")]
		public void Leave ( Client player ) {
			player.GetChar ().lobby.RemovePlayer ( player );
		}

	}
}