namespace TDS.server.instance.lobby {

	using System;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using manager.lobby;
	using manager.utility;
	using player;
	using extend;
	using map;

	partial class Lobby {

		public uint Armor = 100;
		public uint Health = 100;
		private uint Lifes = 1;
		public List<List<Client>> Players = new List<List<Client>> {
			new List<Client> ()
		};
		private List<List<Client>> alivePlayers = new List<List<Client>> {
			new List<Client> ()
		};

		public void AddPlayer ( Client player, bool spectator = false ) {
			player.Freeze ( true );
			Character character = player.GetChar ();
			Lobby oldlobby = character.Lobby;
			if ( oldlobby == MainMenu.TheLobby && this != oldlobby ) {
				oldlobby.Players[0].Remove ( player );
			}
			character.Lobby = this;
			character.Spectating = null;
			player.Dimension = this.GetDimension ();
			if ( this.IsPlayable ) {
				if ( this.GotRounds ) {
					string mapname = this.currentMap != null ? this.currentMap.Name : "unknown";
					player.TriggerEvent ( "onClientPlayerJoinLobby", spectator, mapname, this.Teams, this.teamColorsList,
										this.countdownTime, this.roundTime, this.bombDetonateTime, this.bombPlantTime, this.bombDefuseTime,
										this.RoundEndTime );
				} else {
					player.TriggerEvent ( "onClientPlayerJoinRoundlessLobby" );
					player.Position = this.spawnpoint;
					player.Rotation = this.spawnrotation;
					player.StopSpectating ();
					player.Freeze ( false );
				}
			} else {
				player.Position = this.spawnpoint.Around ( 5 );
				player.StopSpectating ();
			}

			if ( spectator ) {
				this.AddPlayerAsSpectator ( player, character );
			} else {
				this.AddPlayerAsPlayer ( player, character );
			}

			if ( this.IsMapCreateLobby )
				this.StartPlayerFreecam ( player );

			this.SendPlayerRoundInfoOnJoin ( player );
		}

		private void AddPlayerAsSpectator ( Client player, Character character ) {
			this.Players[0].Add ( player );
			character.Team = 0;
			character.Lifes = 0;
		}

		private void AddPlayerAsPlayer ( Client player, Character character ) {
			uint teamID = this.GetTeamIDWithFewestMember ( this.Players );
			this.Players[(int)teamID].Add ( player );
			player.SetSkin ( this.teamSkins[(int)teamID] );
			character.Team = teamID;
			if ( this.IsPlayable && this.GotRounds ) {
				if ( this.countdownTimer != null && this.countdownTimer.isRunning ) {
					this.SetPlayerReadyForRound ( player, teamID );
				} else {
					int teamsinround = this.GetTeamAmountStillInRound ();
					API.ConsoleOutput ( teamsinround + " teams still in round" );
					if ( teamsinround < 2 ) {
						this.EndRoundEarlier ();
						API.ConsoleOutput ( "End round earlier because of joined player" );
					} else {
						this.RespawnPlayerInSpectateMode ( player );
					}
				}
			}
		}

		private void SendPlayerRoundInfoOnJoin ( Client player ) {
			if ( this.currentMap != null ) {
				player.TriggerEvent ( "onClientMapChange", this.currentMap.MapLimits, this.currentMap.MapCenter );
			}
			if ( this.IsPlayable ) {
				this.SendPlayerAmountInFightInfo ( player );
				if ( this.GotRounds ) {
					this.SyncMapVotingOnJoin ( player );
				}
			}

			int tick = Environment.TickCount;
			switch ( this.status ) {
				case "countdown":
					Map map = this.currentMap;
					if ( map != null )
						player.TriggerEvent ( "onClientCountdownStart", map.Name, tick - this.startTick );
					break;
				case "round":
					player.TriggerEvent ( "onClientRoundStart", true, this.Players[0], tick - this.startTick );
					break;
			}
		}

		private void SetPlayerReadyForRound ( Client player, uint teamID ) {
			player.Armor = (int) this.Armor;
			player.Health = (int) this.Health;
			this.Spectate ( player, player );
			if ( teamID > 0 ) {
				Vector3[] spawndata = this.GetMapRandomSpawnData ( teamID );
				player.Position = spawndata[0];
				player.Rotation = spawndata[1];
			}
			player.Freeze ( true );
			this.GivePlayerWeapons ( player );
		}

		public void RemovePlayer ( Client player ) {
			Character character = player.GetChar ();
			uint teamID = character.Team;
			if ( this != MainMenu.TheLobby ) {
				this.SendAllPlayerEvent ( "onClientPlayerLeaveLobby", -1, player );
				if ( this.playersInOwnDimension ) {
					dimensionsUsed.Remove ( player.Dimension );
				}
			}
			this.Players[(int)teamID].Remove ( player );
			if ( character.Lifes > 0 ) {
				this.DmgSys.CheckLastHitter ( player, character, out Client killer );
				this.DeathInfoSync ( player, teamID, killer, unchecked ((int) WeaponHash.Unarmed) );
				this.RemovePlayerFromAlive ( player, character );
			}
			this.DmgSys.PlayerSpree.Remove ( player );

			if ( player.Exists ) {
				player.Transparency = 255;
			}

			if ( this.IsMapCreateLobby )
				this.StopPlayerFreecam ( player, true );

			if ( this != MainMenu.TheLobby ) {
				MainMenu.Join ( player );
			}
		}

		private void RemovePlayerFromAlive ( Client player, Character chara = null ) {
			Character character = chara ?? player.GetChar ();
			int teamID = (int) character.Team;
			character.Lifes = 0;
			int aliveindex = this.alivePlayers[teamID].IndexOf ( player );
			this.PlayerCantBeSpectatedAnymore ( player, aliveindex, teamID );
			this.alivePlayers[teamID].RemoveAt ( aliveindex );
			if ( this.bombAtPlayer == player ) {
				this.DropBomb ();
			}
			this.CheckLobbyForEnoughAlive ();
		}

		private void RespawnPlayerInRound ( Client player ) {
			this.SetPlayerReadyForRound ( player, player.GetChar ().Team );
			player.Freeze ( false );
		}

		private void RespawnPlayerInSpectateMode ( Client player ) {
			player.Position = this.spawnpoint.Around ( 10 );
			player.Freeze ( true );
			this.SpectateTeammate ( player );
			player.TriggerEvent ( "onClientPlayerSpectateMode" );
		}

		private static void OnPlayerRespawn ( Client player ) {
			Character character = player.GetChar ();
			Lobby lobby = character.Lobby;
			if ( lobby.IsPlayable ) {
				if ( character.Lifes > 0 ) {
					lobby.RespawnPlayerInRound ( player );
				} else {
					lobby.RespawnPlayerInSpectateMode ( player );
				}
			} else if ( !lobby.GotRounds ) {
				if ( lobby.spawnpoint != null ) {
					player.Position = lobby.spawnpoint;
					if ( lobby.spawnrotation != null )
						player.Rotation = lobby.spawnrotation;
				}
				player.Freeze ( false );
			} else {
				player.Position = lobby.spawnpoint.Around ( 5 );
				player.Freeze ( true );
			}
		}

		public void OnPlayerDeath ( Client player, Client killer, int weapon, Character character ) {
			if ( character.Lifes > 0 ) {
				character.Lifes--;
				character.Lobby.DeathInfoSync ( player, character.Team, killer, weapon );
				if ( this.bombAtPlayer == player ) {
					this.DropBomb ();
				}
				if ( character.Lifes == 0 ) {
					this.RemovePlayerFromAlive ( player, character );
				}
			}
		}

		private static void OnPlayerDisconnected ( Client player, string reason ) {
			player.GetChar ().Lobby.RemovePlayer ( player );
		}

		private static void OnPlayerWeaponSwitch ( Client player, WeaponHash oldweapon ) {
			Character character = player.GetChar ();
			if ( character.Lobby.bombAtPlayer == player && character.Lobby.currentMap.Type == "bomb" ) {
				character.Lobby.ToggleBombAtHand ( player, oldweapon );
			}
		}

		public void KillPlayer ( Client player, string reason ) {
			player.Kill ();
			player.SendLangNotification ( reason );
		}

		private bool IsSomeoneInLobby () {
			this.RefreshPlayerList ();
			for ( int i = 0; i < this.Players.Count; i++ ) {
				if ( this.Players[i].Count > 0 )
					return true;
			}
			return false;
		}
	}

}
