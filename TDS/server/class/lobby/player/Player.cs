using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
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

		public void AddPlayer ( Client player, bool spectator = false ) {
			player.freeze ( true );
			Class.Character character = player.GetChar ();
			Lobby oldlobby = character.lobby;
			if ( oldlobby == Manager.MainMenu.lobby && this != oldlobby ) {
				oldlobby.players[0].Remove ( player );
			}
			character.lobby = this;
			character.spectating = null;
			player.dimension = this.GetDimension ();
			if ( this.isPlayable ) {
				if ( this.gotRounds ) {
					string mapname = this.currentMap != null ? this.currentMap.name : "unknown";
					player.triggerEvent ( "onClientPlayerJoinLobby", spectator, mapname, this.teams, this.teamColorsList, this.countdownTime, this.roundTime, this.bombDetonateTime, this.bombPlantTime, this.bombDefuseTime, this.roundEndTime );
				} else {
					player.triggerEvent ( "onClientPlayerJoinRoundlessLobby" );
					player.position = this.spawnpoint;
					player.rotation = this.spawnrotation;
					player.stopSpectating ();
					player.freeze ( false );
				}
			} else {
				player.position = this.spawnpoint.Around ( 5 );
				player.stopSpectating ();
			}

			if ( spectator ) {
				this.AddPlayerAsSpectator ( player, character );
			} else {
				this.AddPlayerAsPlayer ( player, character );
			}

			if ( this.isMapCreateLobby )
				this.StartPlayerFreecam ( player );

			this.SendPlayerRoundInfoOnJoin ( player );
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
					API.consoleOutput ( teamsinround + " teams still in round" );
					if ( teamsinround < 2 ) {
						this.EndRoundEarlier ();
						API.consoleOutput ( "End round earlier because of joined player" );
						return;
					} else {
						this.RespawnPlayerInSpectateMode ( player );
					}
				}
			}
		}

		private void SendPlayerRoundInfoOnJoin ( Client player ) {
			if ( this.currentMap != null ) {
				player.triggerEvent ( "onClientMapChange", this.currentMap.mapLimits, this.currentMap.mapCenter );
			}
			if ( this.isPlayable ) {
				this.SendPlayerAmountInFightInfo ( player );
				if ( this.gotRounds ) {
					this.SyncMapVotingOnJoin ( player );
				}
			}

			int tick = Environment.TickCount;
			if ( this.status == "countdown" ) {
				player.triggerEvent ( "onClientCountdownStart", this.currentMap.name, tick - this.startTick );
			} else if ( this.status == "round" ) {
				player.triggerEvent ( "onClientRoundStart", true, this.players[0], tick - this.startTick );
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
			this.GivePlayerWeapons ( player );
		}

		public void RemovePlayer ( Client player ) {
			Character character = player.GetChar ();
			int teamID = character.team;
			if ( this != Manager.MainMenu.lobby ) {
				this.SendAllPlayerEvent ( "onClientPlayerLeaveLobby", -1, player );
				if ( this.playersInOwnDimension ) {
					dimensionsUsed.Remove ( player.dimension );
				}
			}
			this.players[teamID].Remove ( player );
			if ( character.lifes > 0 ) {
				this.damageSys.CheckLastHitter ( player, character, out Client killer );
				Manager.FightInfo.DeathInfoSync ( this, player, teamID, killer, (int) WeaponHash.Unarmed );
				this.RemovePlayerFromAlive ( player, character );
			}
			this.damageSys.playerSpree.Remove ( player );

			if ( player.exists ) {
				player.transparency = 255;
			}

			if ( this.isMapCreateLobby )
				this.StopPlayerFreecam ( player, true );

			if ( this != Manager.MainMenu.lobby ) {
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
			if ( this.bombAtPlayer == player ) {
				this.DropBomb ();
			}
			this.CheckLobbyForEnoughAlive ();
		}

		private void RespawnPlayerInRound ( Client player ) {
			this.SetPlayerReadyForRound ( player, player.GetChar ().team );
			player.freeze ( false );
		}

		private void RespawnPlayerInSpectateMode ( Client player ) {
			player.position = this.spawnpoint.Around ( 10 );
			player.freeze ( true );
			this.SpectateTeammate ( player );
			player.triggerEvent ( "onClientPlayerSpectateMode" );
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
				player.position = lobby.spawnpoint.Around ( 5 );
				player.freeze ( true );
			}
		}

		public void OnPlayerDeath ( Client player, Client killer, int weapon, Character character ) {
			if ( character.lifes > 0 ) {
				character.lifes--;
				character.lobby.DeathInfoSync ( player, character.team, killer, weapon );
				if ( this.bombAtPlayer == player ) {
					this.DropBomb ();
				}
				if ( character.lifes == 0 ) {
					this.RemovePlayerFromAlive ( player, character );
				}
			}
		}

		private static void OnPlayerDisconnected ( Client player, string reason ) {
			player.GetChar ().lobby.RemovePlayer ( player );
		}

		private static void OnPlayerWeaponSwitch ( Client player, WeaponHash oldweapon ) {
			Character character = player.GetChar ();
			if ( character.lobby.bombAtPlayer == player && character.lobby.currentMap.type == "bomb" ) {
				character.lobby.ToggleBombAtHand ( player, oldweapon );
			}
		}

		public void KillPlayer ( Client player, string reason ) {
			player.kill ();
			player.SendLangNotification ( reason );
		}

		private bool IsSomeoneInLobby ( ) {
			this.RefreshPlayerList ();
			for ( int i = 0; i < this.players.Count; i++ ) {
				if ( this.players[i].Count > 0 )
					return true;
			}
			return false;
		}
	}
}