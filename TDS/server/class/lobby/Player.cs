using System;
using System.Collections.Generic;
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

			if ( spectator ) {
				this.AddPlayerAsSpectator ( player, character );
			} else {
				this.AddPlayerAsPlayer ( player, character );
			}

			this.SendPlayerRoundInfoOnJoin ( player );
		}

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
				//int tickremaining = this.countdownTime * 1000 - ( tick - this.startTick );
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
			API.shared.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_IN, 50 );
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
			if ( this != Manager.MainMenu.lobby )
				this.SendAllPlayerEvent ( "onClientPlayerLeaveLobby", -1, player );
			this.players[teamID].Remove ( player );
			if ( character.lifes > 0 ) {
				this.damageSys.CheckLastHitter ( player, character, out Client killer );
				Manager.FightInfo.DeathInfoSync ( this, player, teamID, killer, (int) WeaponHash.Unarmed );
				this.RemovePlayerFromAlive ( player, character );
			}
			this.damageSys.playerSpree.Remove ( player );

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
			player.position = new Vector3 ( Utility.rnd.Next ( -10, 10 ), Utility.rnd.Next ( -10, 10 ), 1000 );
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
				player.position = new Vector3 ( Utility.rnd.Next ( -10, 10 ), Utility.rnd.Next ( -10, 10 ), 1000 );
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

				// BOMB //
				case "onPlayerStartPlanting":
					player.GetChar ().lobby.StartBombPlanting ( player );
					break;

				case "onPlayerStopPlanting":
					player.GetChar ().lobby.StopBombPlanting ( player );
					break;

				case "onPlayerStartDefusing":
					player.GetChar ().lobby.StartBombDefusing ( player );
					break;

				case "onPlayerStopDefusing":
					player.GetChar ().lobby.StopBombDefusing ( player );
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
			this.FuncIterateAllPlayers ( ( player, teamID ) => { 
				this.SetPlayerReadyForRound ( player, teamID );
				player.triggerEvent ( "onClientCountdownStart", this.currentMap.name );
				if ( teamID == 0 )
					this.SpectateAllTeams ( player, true );
			} );
			if ( this.currentMap.type == "bomb" )
				this.GiveBombToRandomTerrorist ();
		}

		internal void FuncIterateAllPlayers ( Action<Client, int> func, int teamID = -1 ) {
			if ( teamID == -1 ) {
				for ( int i = 0; i < this.players.Count; i++ )
					for ( int j = this.players[i].Count - 1; j >= 0; j-- )
						if ( this.players[i][j].exists ) {
							if ( this.players[i][j].GetChar ().lobby == this ) {
								func ( this.players[i][j], i );
							} else
								this.players[i].RemoveAt ( j );
						} else
							this.players[i].RemoveAt ( j );

			} else
				for ( int j = this.players[teamID].Count - 1; j >= 0; j-- )
					if ( this.players[teamID][j].exists ) {
						if ( this.players[teamID][j].GetChar ().lobby == this ) {
							func ( this.players[teamID][j], teamID );
						} else
							this.players[teamID].RemoveAt ( j );
					} else
						this.players[teamID].RemoveAt ( j );

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