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

		private static void OnPlayerDeathOtherTask ( Client player, NetHandle entityKiller, int weapon ) {
			if ( !deadTimer.ContainsKey ( player ) ) {
				Character character = player.GetChar ();
				Damagesys dmgsys = character.lobby.damageSys;

				API.shared.sendNativeToPlayer ( player, Hash._DISABLE_AUTOMATIC_RESPAWN, true );
				API.shared.sendNativeToPlayer ( player, Hash.IGNORE_NEXT_RESTART, true );
				API.shared.sendNativeToPlayer ( player, Hash.SET_FADE_OUT_AFTER_DEATH, false );
				API.shared.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_OUT, 2000 );

				player.freeze ( true );
				deadTimer.TryAdd ( player, Timer.SetTimer ( ( ) => SpawnAfterDeath ( player ), 2000, 1 ) );
				Client killer = API.shared.getPlayerFromHandle ( entityKiller );

				if ( character.lifes > 0 ) {
					character.lobby.OnPlayerDeath ( player, entityKiller, weapon, character );

					// Kill //
					if ( killer != null ) {
						if ( character.lobby.isOfficial ) {
							killer.GetChar ().kills++;
						}
						if ( !dmgsys.playerKills.ContainsKey ( killer ) ) {
							dmgsys.playerKills.TryAdd ( killer, 0 );
						}
						dmgsys.playerKills[killer]++;
					} else {
						character.lobby.damageSys.CheckLastHitter ( player, character );
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
		
		private static void OnPlayerDeath ( Client player, NetHandle entityKiller, int weapon ) {
			Task.Run ( () => {
				OnPlayerDeathOtherTask ( player, entityKiller, weapon );
			} );
		}

		private static void SpawnAfterDeath ( Client player ) {
			deadTimer.TryRemove ( player, out Timer timer );
			timer.Kill ();
			if ( player.exists ) {
				API.shared.sendNativeToPlayer ( player, Hash._RESET_LOCALPLAYER_STATE, player );
				API.shared.sendNativeToPlayer ( player, Hash.NETWORK_REQUEST_CONTROL_OF_ENTITY, player );
				API.shared.sendNativeToPlayer ( player, Hash.NETWORK_RESURRECT_LOCAL_PLAYER, 0, 0, 2000, player.rotation.Z, false, false );
				API.shared.sendNativeToPlayer ( player, Hash.RESURRECT_PED, player );
				API.shared.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_IN, 2000 );
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

		public void CheckLastHitter ( Client player, Character character ) {
			if ( this.lastHitterDictionary.ContainsKey ( player ) ) {
				this.lastHitterDictionary.TryRemove ( player, out Client lasthitter );
				if ( lasthitter.exists ) {
					Character lasthittercharacter = lasthitter.GetChar ();
					if ( character.lobby == lasthittercharacter.lobby ) {
						if ( lasthittercharacter.lifes > 0 ) {
							lasthittercharacter.kills++;
							lasthitter.SendLangNotification ( "got_last_hitted_kill", player.name );
						}
					}
				}
			}
			
		}
	}
}