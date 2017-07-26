﻿using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace Class {
	partial class Damagesys : Script {

		private void OnPlayerDeath ( Client player, NetHandle entityKiller, int weapon ) {
			Character character = player.GetChar ();
			API.TriggerClientEventForLobby ( character.lobby, "onClientPlayerDeath", -1, player );

			API.sendNativeToPlayer ( player, Hash._DISABLE_AUTOMATIC_RESPAWN, true );
			API.sendNativeToPlayer ( player, Hash.IGNORE_NEXT_RESTART, true );
			API.sendNativeToPlayer ( player, Hash.SET_FADE_OUT_AFTER_DEATH, false );
			API.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_OUT, 2000 );

			player.freeze ( true );
			Timer.SetTimer ( ( ) => SpawnAfterDeath ( player ), 2000, 1 );
			Client killer = API.getPlayerFromHandle ( entityKiller );

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
					CheckForAssist ( player, character );
			}
		}

		private void SpawnAfterDeath ( Client player ) {
			if ( player.exists ) {
				API.sendNativeToPlayer ( player, Hash._RESET_LOCALPLAYER_STATE, player );
				API.sendNativeToPlayer ( player, Hash.NETWORK_REQUEST_CONTROL_OF_ENTITY, player );
				API.sendNativeToPlayer ( player, Hash.NETWORK_RESURRECT_LOCAL_PLAYER, 0, 0, 2000, player.rotation.Z, false, false );
				API.sendNativeToPlayer ( player, Hash.RESURRECT_PED, player );
				API.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_IN, 2000 );
			}
		}

		private static void CheckForAssist ( Client player, Character character ) {
			if ( allHitters.ContainsKey ( player ) ) {
				int halfarmorhp = ( character.lobby.armor + character.lobby.health ) / 2;
				foreach ( KeyValuePair<Client, int> entry in allHitters[player] ) {
					if ( entry.Value >= halfarmorhp ) {
						Character targetcharacter = entry.Key.GetChar ();
						if ( entry.Key.exists && targetcharacter.lobby == character.lobby ) {
							targetcharacter.assists++;
							entry.Key.SendLangNotification ( "got_assist", player.name );
						}
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