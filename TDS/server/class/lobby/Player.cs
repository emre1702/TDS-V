using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace Class {

	partial class Lobby : Script {

		private int armor = 100;
		private int health = 100;
		private int lifes = 1;
		private List<List<Client>> players = new List<List<Client>> { new List<Client> () };
		private List<List<Client>> alivePlayers = new List<List<Client>> { new List<Client> () };

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
			API.removeAllPlayerWeapons ( player );
		}

		private void GivePlayerWeapons ( Client player ) {
			for ( int i = 0; i < this.weapons.Count; i++ ) {
				API.givePlayerWeapon ( player, this.weapons[i], this.weaponsAmmo[i], false, true );
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
				this.CheckLobbyForEnoughAlive ();
			}
			Manager.MainMenu.Join ( player );
		}

		private void RespawnPlayerInRound ( Client player ) {
			this.SetPlayerReadyForRound ( player, player.GetChar ().team );
			player.freeze ( false );
		}

		private void RespawnPlayerInSpectateMode ( Client player ) {
			player.position = new Vector3 ( rnd.Next ( -10, 10 ), rnd.Next ( -10, 10 ), 1000 );
			player.freeze ( true );
			this.SpectateTeammate ( player );
			player.triggerEvent ( "onClientSpectateMode" );
		}

		private void OnPlayerRespawn ( Client player ) {
			Class.Character character = player.GetChar ();
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
			}
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

		private void KillPlayer ( Client player, string reason ) {
			player.kill ();
			player.SendLangNotification ( reason );
		}
	}
}