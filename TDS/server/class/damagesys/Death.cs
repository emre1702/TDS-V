using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace Class {
	partial class Damagesys {

		private static Dictionary<Client, Timer> deadTimer = new Dictionary<Client, Timer> (); 

		private static void OnPlayerDeath ( Client player, NetHandle entityKiller, int weapon ) {
			if ( !deadTimer.ContainsKey ( player ) ) {
				Character character = player.GetChar ();
				API.shared.TriggerClientEventForLobby ( character.lobby, "onClientPlayerDeath", -1, player );

				API.shared.sendNativeToPlayer ( player, Hash._DISABLE_AUTOMATIC_RESPAWN, true );
				API.shared.sendNativeToPlayer ( player, Hash.IGNORE_NEXT_RESTART, true );
				API.shared.sendNativeToPlayer ( player, Hash.SET_FADE_OUT_AFTER_DEATH, false );
				API.shared.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_OUT, 2000 );

				player.freeze ( true );
				deadTimer[player] = Timer.SetTimer ( ( ) => SpawnAfterDeath ( player ), 2000, 1 );
				Client killer = API.shared.getPlayerFromHandle ( entityKiller );

				if ( character.lifes > 0 ) {
					character.lobby.OnPlayerDeath ( player, entityKiller, weapon, character );

					// Kill //
					if ( killer != null ) {
						Console.WriteLine ( player.name + " got killed by " + killer.name );
						if ( character.lobby == Manager.Arena.lobby )
							killer.GetChar ().kills++;
					} else {
						CheckLastHitter ( player, character );
						Console.WriteLine ( player.name + " died" );
					}

					// Death //
					if ( character.lobby == Manager.Arena.lobby )
						character.deaths++;

					// Assist //
					if ( character.lobby == Manager.Arena.lobby )
						CheckForAssist ( player, character, killer );
				}
			}
		}

		private static void SpawnAfterDeath ( Client player ) {
			deadTimer.Remove ( player );
			if ( player.exists ) {
				API.shared.sendNativeToPlayer ( player, Hash._RESET_LOCALPLAYER_STATE, player );
				API.shared.sendNativeToPlayer ( player, Hash.NETWORK_REQUEST_CONTROL_OF_ENTITY, player );
				API.shared.sendNativeToPlayer ( player, Hash.NETWORK_RESURRECT_LOCAL_PLAYER, 0, 0, 2000, player.rotation.Z, false, false );
				API.shared.sendNativeToPlayer ( player, Hash.RESURRECT_PED, player );
				API.shared.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_IN, 2000 );
			}
		}

		private static void CheckForAssist ( Client player, Character character, Client killer ) {
			if ( allHitters.ContainsKey ( player ) ) {
				int halfarmorhp = ( character.lobby.armor + character.lobby.health ) / 2;
				foreach ( KeyValuePair<Client, int> entry in allHitters[player] ) {
					if ( entry.Value >= halfarmorhp ) {
						Character targetcharacter = entry.Key.GetChar ();
						if ( entry.Key.exists && targetcharacter.lobby == character.lobby && killer != entry.Key ) {
							targetcharacter.assists++;
							entry.Key.SendLangNotification ( "got_assist", player.name );
						}
						if ( halfarmorhp % 2 != 0 || entry.Value != halfarmorhp / 2 )
							return;
					}
				}
			}
		}

		public static void CheckLastHitter ( Client player, Character character ) {
			if ( lastHitterDictionary.ContainsKey ( player ) ) {
				Client lasthitter = lastHitterDictionary[player];
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
			lastHitterDictionary.Remove ( player );
		}
	}
}