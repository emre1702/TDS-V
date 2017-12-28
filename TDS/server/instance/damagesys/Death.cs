﻿namespace TDS.server.instance.damagesys {

	using System.Collections.Generic;
	using GTANetworkAPI;
	using manager.utility;
	using player;
	using extend;
	using utility;
    using TDS.server.instance.lobby;

    partial class Damagesys {

		private static readonly Dictionary<NetHandle, Timer> sDeadTimer = new Dictionary<NetHandle, Timer> ();
		public Dictionary<NetHandle, uint> PlayerAssists = new Dictionary<NetHandle, uint> (),
										PlayerKills = new Dictionary<NetHandle, uint> ();

		private void OnPlayerDeath ( Client player, NetHandle entityKiller, uint weapon, CancelEventArgs cancel ) {
            cancel.Cancel = true;

            if ( !sDeadTimer.ContainsKey ( player ) ) {
				Character character = player.GetChar ();

                if ( !( character.Lobby is FightLobby lobby ) )
                    return;

				Damagesys dmgsys = lobby.DmgSys;

				NAPI.ClientEvent.TriggerClientEvent ( player, "clientPlayerDeathNatives" );

				player.Freeze ( true );
                sDeadTimer.TryAdd ( player, Timer.SetTimer ( () => SpawnAfterDeath ( player ), 2000 ) );
                Client killer = NAPI.Player.GetPlayerFromHandle ( entityKiller );
                killer = killer.Exists ? killer : dmgsys.GetLastHitter ( player, character );

				dmgsys.PlayerSpree.Remove ( player );

				if ( character.Lifes > 0 ) {
                    lobby.OnPlayerDeath ( player, killer, weapon, character );

					// Kill //
					if ( killer != null ) {
						if ( character.Lobby.IsOfficial )
							killer.GetChar ().Kills++;
						if ( !dmgsys.PlayerKills.ContainsKey ( killer ) )
							dmgsys.PlayerKills.TryAdd ( killer, 0 );
						dmgsys.PlayerKills[killer]++;

						// Killingspree //
						dmgsys.AddToKillingSpree ( killer );
					}

					if ( character.Lobby.IsOfficial ) {
						// Death //
						character.Deaths++;
						// Assist //
						dmgsys.CheckForAssist ( player, character, killer );
					}
				}
			}
		}

		private static void SpawnAfterDeath ( Client player ) {
            sDeadTimer.Remove ( player, out Timer timer );
			timer.Kill ();
			if ( player.Exists )
				NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerRespawn" );
		}

		private void CheckForAssist ( Client player, Character character, Client killer ) {
			if ( AllHitters.ContainsKey ( player ) ) {
				uint halfarmorhp = ( lobby.Armor + lobby.Health ) / 2;
				foreach ( KeyValuePair<NetHandle, int> entry in AllHitters[player] ) {
					Client target = NAPI.Player.GetPlayerFromHandle ( entry.Key );
					if ( entry.Value >= halfarmorhp ) {
						Character targetcharacter = target.GetChar ();
						if ( target.Exists && targetcharacter.Lobby == character.Lobby && killer != target ) {
							targetcharacter.Assists++;
							target.SendLangNotification ( "got_assist", player.Name );
							if ( !PlayerAssists.ContainsKey ( target ) )
								PlayerAssists[target] = 0;
							PlayerAssists[target]++;
						}
						if ( killer != target ||
							halfarmorhp % 2 != 0 ||
							entry.Value != halfarmorhp / 2 ||
							AllHitters[player].Count > 2 )
							return;
					}
				}
			}
		}

		public void CheckLastHitter ( Client player, Character character, out Client lastHitter ) {
			if ( LastHitterDictionary.ContainsKey ( player ) ) {
				LastHitterDictionary.Remove ( player, out NetHandle lastHitterhandle );
                lastHitter = NAPI.Player.GetPlayerFromHandle ( lastHitterhandle );
                if ( lastHitter.Exists ) {
					Character lasthittercharacter = lastHitter.GetChar ();
					if ( character.Lobby == lasthittercharacter.Lobby )
						if ( lasthittercharacter.Lifes > 0 ) {
							lasthittercharacter.Kills++;
							lastHitter.SendLangNotification ( "got_last_hitted_kill", player.Name );
							AddToKillingSpree ( lastHitter );
						}
				}
			} else
				lastHitter = null;
		}

		public Client GetLastHitter ( Client player, Character character ) {
			if ( LastHitterDictionary.ContainsKey ( player ) ) {
				LastHitterDictionary.Remove ( player, out NetHandle lasthitterhandle );
                Client lasthitter = NAPI.Player.GetPlayerFromHandle ( lasthitterhandle );
                if ( lasthitter.Exists ) {
					Character lasthittercharacter = lasthitter.GetChar ();
					if ( character.Lobby == lasthittercharacter.Lobby )
						return lasthitter;
				}
			}
			return null;
		}
	}

}
