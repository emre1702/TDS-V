namespace TDS.server.instance.damagesys {

	using System.Collections.Generic;
	using GTANetworkAPI;
	using manager.utility;
	using player;
	using extend;
	using utility;

	internal partial class Damagesys {

		private static readonly Dictionary<Client, Timer> deadTimer = new Dictionary<Client, Timer> ();
		public Dictionary<Client, uint> PlayerAssists = new Dictionary<Client, uint> (),
										PlayerKills = new Dictionary<Client, uint> ();

		private void OnPlayerDeath ( Client player, NetHandle entityKiller, int weapon ) {
			if ( !deadTimer.ContainsKey ( player ) ) {
				Character character = player.GetChar ();
				Damagesys dmgsys = character.Lobby.DmgSys;

				player.TriggerEvent ( "clientPlayerDeathNatives" );

				player.Freeze ( true );
				deadTimer.TryAdd ( player, Timer.SetTimer ( () => SpawnAfterDeath ( player ), 2000 ) );
				Client killer = this.API.GetPlayerFromHandle ( entityKiller ) ??
								character.Lobby.DmgSys.GetLastHitter ( player, character );

				dmgsys.PlayerSpree.Remove ( player );

				if ( character.Lifes > 0 ) {
					character.Lobby.OnPlayerDeath ( player, killer, weapon, character );

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
						character.Lobby.DmgSys.CheckForAssist ( player, character, killer );
					}
				}
			}
		}

		private static void SpawnAfterDeath ( Client player ) {
			deadTimer.Remove ( player, out Timer timer );
			timer.Kill ();
			if ( player.Exists )
				player.TriggerEvent ( "onClientPlayerRespawn" );
		}

		private void CheckForAssist ( Client player, Character character, Client killer ) {
			if ( this.AllHitters.ContainsKey ( player ) ) {
				uint halfarmorhp = ( character.Lobby.Armor + character.Lobby.Health ) / 2;
				foreach ( KeyValuePair<Client, int> entry in this.AllHitters[player] ) {
					Client target = entry.Key;
					if ( entry.Value >= halfarmorhp ) {
						Character targetcharacter = entry.Key.GetChar ();
						if ( target.Exists && targetcharacter.Lobby == character.Lobby && killer != target ) {
							targetcharacter.Assists++;
							target.SendLangNotification ( "got_assist", player.Name );
							Damagesys dmgsys = character.Lobby.DmgSys;
							if ( !dmgsys.PlayerAssists.ContainsKey ( target ) )
								dmgsys.PlayerAssists[target] = 0;
							dmgsys.PlayerAssists[target]++;
						}
						if ( killer != target ||
							halfarmorhp % 2 != 0 ||
							entry.Value != halfarmorhp / 2 ||
							this.AllHitters[player].Count > 2 )
							return;
					}
				}
			}
		}

		public void CheckLastHitter ( Client player, Character character, out Client lastHitter ) {
			if ( this.LastHitterDictionary.ContainsKey ( player ) ) {
				this.LastHitterDictionary.Remove ( player, out lastHitter );
				if ( lastHitter.Exists ) {
					Character lasthittercharacter = lastHitter.GetChar ();
					if ( character.Lobby == lasthittercharacter.Lobby )
						if ( lasthittercharacter.Lifes > 0 ) {
							lasthittercharacter.Kills++;
							lastHitter.SendLangNotification ( "got_last_hitted_kill", player.Name );
							this.AddToKillingSpree ( lastHitter );
						}
				}
			} else
				lastHitter = null;
		}

		public Client GetLastHitter ( Client player, Character character ) {
			if ( this.LastHitterDictionary.ContainsKey ( player ) ) {
				this.LastHitterDictionary.Remove ( player, out Client lasthitter );
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
