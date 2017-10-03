using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Manager;

namespace Class {
	partial class Damagesys {

		private static ConcurrentDictionary<Client, Timer> deadTimer = new ConcurrentDictionary<Client, Timer> ();
		public ConcurrentDictionary<Client, int> playerKills = new ConcurrentDictionary<Client, int> ();
		public ConcurrentDictionary<Client, int> playerAssists = new ConcurrentDictionary<Client, int> ();

		private void OnPlayerDeath ( Client player, NetHandle entityKiller, int weapon ) {
			if ( !deadTimer.ContainsKey ( player ) ) {
				Character character = player.GetChar ();
				Damagesys dmgsys = character.lobby.damageSys;

				player.triggerEvent ( "clientPlayerDeathNatives" );

				player.freeze ( true );
				deadTimer.TryAdd ( player, Timer.SetTimer ( ( ) => SpawnAfterDeath ( player ), 2000, 1 ) );
				Client killer = API.getPlayerFromHandle ( entityKiller ) ?? character.lobby.damageSys.GetLastHitter ( player, character );

				dmgsys.playerSpree.Remove ( player );

				if ( character.lifes > 0 ) {
					character.lobby.OnPlayerDeath ( player, killer, weapon, character );

					// Kill //
					if ( killer != null ) {
						if ( character.lobby.isOfficial ) {
							killer.GetChar ().kills++;
						}
						if ( !dmgsys.playerKills.ContainsKey ( killer ) ) {
							dmgsys.playerKills.TryAdd ( killer, 0 );
						}
						dmgsys.playerKills[killer]++;

						// Killingspree //
						dmgsys.AddToKillingSpree ( killer );
					}

					if ( character.lobby.isOfficial ) {
						// Death //
						character.deaths++;
						// Assist //
						character.lobby.damageSys.CheckForAssist ( player, character, killer );
					}

				}
			}
		}

		private static void SpawnAfterDeath ( Client player ) {
			deadTimer.TryRemove ( player, out Timer timer );
			timer.Kill ();
			if ( player.exists ) {
				player.triggerEvent ( "onClientPlayerRespawn" );
			}
		}

		private void CheckForAssist ( Client player, Character character, Client killer ) {
			if ( this.allHitters.ContainsKey ( player ) ) {
				int halfarmorhp = ( character.lobby.armor + character.lobby.health ) / 2;
				foreach ( KeyValuePair<Client, int> entry in this.allHitters[player] ) {
					Client target = entry.Key;
					if ( entry.Value >= halfarmorhp ) {
						Character targetcharacter = entry.Key.GetChar ();
						if ( target.exists && targetcharacter.lobby == character.lobby && killer != target ) {
							targetcharacter.assists++;
							target.SendLangNotification ( "got_assist", player.name );
							Damagesys dmgsys = character.lobby.damageSys;
							if ( !dmgsys.playerAssists.ContainsKey ( target ) ) {
								dmgsys.playerAssists[target] = 0;
							}
							dmgsys.playerAssists[target]++;
						}
						if ( killer != target || halfarmorhp % 2 != 0 || entry.Value != halfarmorhp / 2 || this.allHitters[player].Count > 2 )
							return;
					}
				}
			}
		}

		public void CheckLastHitter ( Client player, Character character, out Client lasthitter ) {
			if ( this.lastHitterDictionary.ContainsKey ( player ) ) {
				this.lastHitterDictionary.TryRemove ( player, out lasthitter );
				if ( lasthitter.exists ) {
					Character lasthittercharacter = lasthitter.GetChar ();
					if ( character.lobby == lasthittercharacter.lobby ) {
						if ( lasthittercharacter.lifes > 0 ) {
							lasthittercharacter.kills++;
							lasthitter.SendLangNotification ( "got_last_hitted_kill", player.name );
							this.AddToKillingSpree ( lasthitter );
						}
					}
				}
			} else
				lasthitter = null;

		}

		public Client GetLastHitter ( Client player, Character character ) {
			if ( this.lastHitterDictionary.ContainsKey ( player ) ) {
				this.lastHitterDictionary.TryRemove ( player, out Client lasthitter );
				if ( lasthitter.exists ) {
					Character lasthittercharacter = lasthitter.GetChar ();
					if ( character.lobby == lasthittercharacter.lobby ) {
						return lasthitter;
					}
				}
			}
			return null;
		}
	}
}