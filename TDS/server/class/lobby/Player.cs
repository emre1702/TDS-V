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
		public List<List<Client>> players = new List<List<Client>> { new List<Client> () };
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

		private void AddPlayerAsSpectator ( Client player, Character character ) {
			this.players[0].Add ( player );
			character.team = 0;
			character.lifes = 0;
		}

		private void AddPlayerAsPlayer ( Client player, Character character ) {
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

		public void AddPlayer ( Client player, bool spectator = false ) {
			player.freeze ( true );
			Class.Character character = player.GetChar ();
			Lobby oldlobby = character.lobby;
			if ( oldlobby == Manager.MainMenu.lobby && this != oldlobby ) {
				oldlobby.players[0].Remove ( player );
			}
			character.lobby = this;
			character.spectating = null;
			player.dimension = this.dimension;
			if ( this.isPlayable ) {
				if ( this.gotRounds ) {
					string mapname = this.currentMap != null ? this.currentMap.name : "unknown";
					player.triggerEvent ( "onClientPlayerJoinLobby", spectator, this.countdownTime, this.roundTime, mapname, this.teams, this.teamColorsList );
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
			}

			if ( this.currentMap != null && this.currentMap.mapLimits.Count > 0 ) {
				player.triggerEvent ( "sendClientMapData", this.currentMap.mapLimits );
			}

			if ( spectator ) {
				this.AddPlayerAsSpectator ( player, character );
			} else {
				this.AddPlayerAsPlayer ( player, character );
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
			this.GivePlayerWeapons ( player );
		}

		private void GivePlayerWeapons ( Client player ) {
			API.shared.removeAllPlayerWeapons ( player );
			for ( int i = 0; i < this.weapons.Count; i++ ) {
				API.shared.givePlayerWeapon ( player, this.weapons[i], this.weaponsAmmo[i], false, true );
			}
		}

		public void RemovePlayer ( Client player ) {
			Character character = player.GetChar ();
			int teamID = character.team;
			this.players[teamID].Remove ( player );
			if ( character.lifes > 0 )
				this.RemovePlayerFromAlive ( player, character );

			if ( this != Manager.MainMenu.lobby ) {
				player.triggerEvent ( "onClientPlayerLeaveLobby" );
				Manager.MainMenu.Join ( player );
			}
		}

		private void RemovePlayerFromAlive ( Client player, Character chara = null ) {
			Character character = chara ?? player.GetChar ();
			int teamID = character.team;
			character.lifes = 0;
			int aliveindex = this.alivePlayers[teamID].IndexOf ( player );
			this.PlayerCantBeSpectatedAnymore ( player, aliveindex, teamID );
			this.alivePlayers[teamID].RemoveAt ( aliveindex );
			this.damageSys.CheckLastHitter ( player, character );
			this.CheckLobbyForEnoughAlive ();
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
				character.lobby.DeathInfoSync ( player, character.team, API.shared.getEntityFromHandle<Client> ( killer ), weapon );
				if ( character.lifes == 0 ) {
					this.RemovePlayerFromAlive ( player, character );
				}
			}
		}

		private static void OnPlayerDisconnected ( Client player, string reason ) {
			player.GetChar ().lobby.RemovePlayer ( player );
		}

		private static void OnClientEventTrigger ( Client player, string eventName, params dynamic[] args ) {
			switch ( eventName ) {

				case "joinLobby":
					API.shared.consoleOutput ( "joinLobby event" );
					if ( lobbysbyindex.ContainsKey ( args[0] ) ) {
						Lobby lobby = lobbysbyindex[args[0]];
						lobby.AddPlayer ( player, args[1] );
					} else {
						/* player.sendNotification (  lobby doesn't exist ); */
						player.triggerEvent ( "onClientJoinMainMenu" );
					}
					break;

				case "spectateNext":
					API.shared.consoleOutput ( "spectateNext event" );
					Class.Character character = player.GetChar ();
					if ( character.lifes == 0 && ( character.lobby.status == "round" || character.team == 0 && character.lobby.status == "countdown" ) ) {
						if ( character.team == 0 )
							character.lobby.SpectateAllTeams ( player, args[0] );
						else
							character.lobby.SpectateTeammate ( player, args[0] );
					}
					break;

				case "onPlayerWasTooLongOutsideMap":
					API.shared.consoleOutput ( "onPlayerWasTooLongOutsideMap event" );
					Class.Character character2 = player.GetChar ();
					if ( character2.lobby.isPlayable ) {
						character2.lobby.KillPlayer ( player, "too_long_outside_map" );
					}
					break;

				case "onMapMenuOpen":
					API.shared.consoleOutput ( "onMapMenuOpen event" );
					player.GetChar ().lobby.SendMapsForVoting ( player );
					break;

				case "onMapVotingRequest":
					API.shared.consoleOutput ( "onMapVotingRequest event" );
					player.GetChar ().lobby.AddMapToVoting ( player, args[0] );
					break;

				case "onVoteForMap":
					API.shared.consoleOutput ( "onVoteForMap event" );
					player.GetChar ().lobby.AddVoteToMap ( player, args[0] );
					break;
			}
		}

		public void KillPlayer ( Client player, string reason ) {
			player.kill ();
			player.SendLangNotification ( reason );
		}

		private void RewardAllPlayer ( ) {
			foreach ( KeyValuePair<Client, int> entry in this.damageSys.playerDamage ) {
				if ( entry.Key.exists ) {
					Client player = entry.Key;
					Character character = player.GetChar ();
					if ( character.lobby == this ) {
						List<int> reward = new List<int> ();
						if ( this.damageSys.playerKills.ContainsKey ( player ) ) {
							reward.Add ( (int) ( Manager.Money.moneyForDict["kill"] * (double) this.damageSys.playerKills[player] ) );
						} else
							reward.Add ( 0 );
						if ( this.damageSys.playerAssists.ContainsKey ( player ) ) {
							reward.Add ( (int) ( Manager.Money.moneyForDict["assist"] * (double) this.damageSys.playerAssists[player] ) );
						} else
							reward.Add ( 0 );
						reward.Add ( (int) ( Manager.Money.moneyForDict["damage"] * (double) entry.Value ) );

						int total = reward[0] + reward[1] + reward[2];
						player.GiveMoney ( total, character );
						player.SendLangNotification ( "round_reward", reward[0].ToString (), reward[1].ToString (), reward[2].ToString (), total.ToString() );
					}
				}
			}
		}

		private void SetAllPlayersInCountdown ( ) {
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