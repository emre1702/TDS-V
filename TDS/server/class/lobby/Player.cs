﻿using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Manager;

namespace Class {

	partial class Lobby {

		public int armor = 100;
		public int health = 100;
		private int lifes = 1;
		private List<List<Client>> players = new List<List<Client>> { new List<Client> () };
		private List<List<Client>> alivePlayers = new List<List<Client>> { new List<Client> () };

		private void SendPlayerAmountInFightInfo ( Client player ) {
			List<int> amountinteams = new List<int> ();
			List<int> amountaliveinteams = new List<int> ();
			for ( int i = 0; i < this.players.Count; i++ ) {
				amountinteams.Add ( this.players[i].Count );
				amountaliveinteams.Add ( this.alivePlayers[i].Count );
			}
			player.PlayerAmountInFightSync ( amountinteams, amountaliveinteams );
		}

		public void AddPlayer ( Client player, bool spectator = false ) {
			if ( this != MainMenu.lobby )
				MainMenu.lobby.players[0].Remove ( player );
			player.freeze ( true );
			Class.Character character = player.GetChar ();
			character.lobby = this;
			character.spectating = null;
			player.dimension = this.dimension;
			if ( this.isPlayable ) {
				if ( this.gotRounds ) {
					player.triggerEvent ( "onClientPlayerJoinLobby", spectator, this.countdownTime, this.roundTime, ( this.currentMap.created != false ? this.currentMap.name : "unknown" ), this.teams, this.teamColorsList );
					this.SyncMapVotingOnJoin ( player );
				} else {
					player.triggerEvent ( "onClientPlayerJoinRoundlessLobby" );
					player.position = this.spawnpoint;
					player.rotation = this.spawnrotation;
					player.stopSpectating ();
					player.freeze ( false );
				}
			} else {
				player.position = new Vector3 ( Manager.Utility.rnd.Next ( -10, 10 ), Manager.Utility.rnd.Next ( -10, 10 ), 1000 );
				player.stopSpectating ();
				if ( this == Manager.MainMenu.lobby )
					player.triggerEvent ( "onClientPlayerLeaveLobby" );
			}

			if ( this.currentMap.created != false && this.currentMap.mapLimits.Count > 0 ) {
				player.triggerEvent ( "sendClientMapData", this.currentMap.mapLimits );
			}
			if ( spectator ) {
				this.players[0].Add ( player );
				character.team = 0;
				character.lifes = 0;
			} else {
				int teamID = this.GetTeamIDWithFewestMember ( this.players );
				this.players[teamID].Add ( player );
				player.setSkin ( this.teamSkins[teamID] );
				character.team = teamID;
				if ( this.isPlayable && this.gotRounds ) {
					if ( this.countdownTimer != null && this.countdownTimer.isRunning ) {
						this.SetPlayerReadyForRound ( player, teamID );
					} else {
						int teamsinround = this.GetTeamAmountStillInRound ();
						API.shared.consoleOutput ( teamsinround + " teams still in round" );
						if ( teamsinround < 2 ) {
							this.EndRoundEarlier ();
							API.shared.consoleOutput ( "End round earlier because of joined player" );
							return;
						} else {
							this.RespawnPlayerInSpectateMode ( player );
						}	
					}
				}
			}
			this.SendPlayerAmountInFightInfo ( player );
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
				this.damageSys.CheckLastHitter ( player, character );
				this.CheckLobbyForEnoughAlive ();
			}
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
			} else if ( !lobby.gotRounds ) {
				if ( lobby.spawnpoint != null ) {
					player.position = lobby.spawnpoint;
					if ( lobby.spawnrotation != null )
						player.rotation = lobby.spawnrotation;
				}
				player.freeze ( false );
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
				lobby.damageSys.CheckLastHitter ( player, character );
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

				case "onVoteForMap":
					player.GetChar ().lobby.AddVoteToMap ( player, args[0] );
					break;
			}
		}

		public void KillPlayer ( Client player, string reason ) {
			player.kill ();
			player.SendLangNotification ( reason );
		}

		private void RewardAllPlayer ( ) {
			foreach ( KeyValuePair<Client, double> entry in this.damageSys.playerDamage ) {
				if ( entry.Key.exists ) {
					Client player = entry.Key;
					Character character = player.GetChar ();
					if ( character.lobby == this ) {
						List<int> reward = new List<int> ();
						if ( this.damageSys.playerKills.ContainsKey ( player ) ) {
							reward.Add ( (int) ( Manager.Money.moneyForDict["kill"] * this.damageSys.playerKills[player] ) );
						} else
							reward.Add ( 0 );
						if ( this.damageSys.playerAssists.ContainsKey ( player ) ) {
							reward.Add ( (int) ( Manager.Money.moneyForDict["assist"] * this.damageSys.playerAssists[player] ) );
						} else
							reward.Add ( 0 );
						API.shared.consoleOutput ( entry.Value + " Damage" );
						reward.Add ( (int) ( Manager.Money.moneyForDict["damage"] * entry.Value ) );

						int total = reward[0] + reward[1] + reward[2];
						player.GiveMoney ( total, character );
						player.SendLangNotification ( "round_reward", reward[0].ToString (), reward[1].ToString (), reward[2].ToString (), total.ToString() );
					}
				}
			}

			this.damageSys.allHitters = new Dictionary<Client, Dictionary<Client, int>> ();
			this.damageSys.lastHitterDictionary = new Dictionary<Client, Client> ();
			this.damageSys.playerDamage = new Dictionary<Client, double> ();
			this.damageSys.playerKills = new Dictionary<Client, double> ();
			this.damageSys.playerAssists = new Dictionary<Client, double> ();
		}

		private void SendPlayerRoundCountdownInfo ( ) {
			this.spectatingMe = new Dictionary<Client, List<Client>> ();
			for ( int i = 0; i < this.players.Count; i++ )
				for ( int j = 0; j < this.players[i].Count; j++ ) {
					this.SetPlayerReadyForRound ( this.players[i][j], i );
					API.shared.sendNativeToPlayer ( this.players[i][j], Hash.DO_SCREEN_FADE_IN, this.countdownTime * 1000 );
					this.players[i][j].triggerEvent ( "onClientCountdownStart", this.currentMap.name );
					if ( i == 0 )
						this.SpectateAllTeams ( this.players[i][j], true );
				}
		}

	}
}