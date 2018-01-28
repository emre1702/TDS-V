namespace TDS.server.instance.damagesys {

	using System.Collections.Generic;
	using GTANetworkAPI;
	using manager.utility;
	using player;
	using extend;
	using utility;
    using TDS.server.instance.lobby;

    partial class Damagesys {

		private static readonly Dictionary<Client, Timer> sDeadTimer = new Dictionary<Client, Timer> ();
		public Dictionary<Client, uint> PlayerAssists = new Dictionary<Client, uint> (),
										PlayerKills = new Dictionary<Client, uint> ();

		private void OnPlayerDeath ( Client player, Client killer, uint weapon, CancelEventArgs cancel ) {
            cancel.Spawn = false;
            if ( !sDeadTimer.ContainsKey ( player ) ) {
				Character character = player.GetChar ();

                player.Freeze ( true );
                sDeadTimer.TryAdd ( player, Timer.SetTimer ( ( ) => SpawnAfterDeath ( player ), 2000 ) );

                if ( !( character.Lobby is FightLobby lobby ) )
                    return;

				Damagesys dmgsys = lobby.DmgSys;

                killer = dmgsys.GetKiller ( player, killer, character );

				dmgsys.PlayerSpree.Remove ( player );

				if ( character.Lifes > 0 ) {
                    lobby.OnPlayerDeath ( player, killer, weapon, character );

					// Kill //
					if ( killer != player ) {
						if ( character.Lobby is Arena )
							killer.GetChar ().GiveKill();
						if ( !dmgsys.PlayerKills.ContainsKey ( killer ) )
							dmgsys.PlayerKills.TryAdd ( killer, 0 );
						dmgsys.PlayerKills[killer]++;

						// Killingspree //
						dmgsys.AddToKillingSpree ( killer );
					}

					if ( character.Lobby is Arena ) 
						// Death //
						character.GiveDeath();

                    // Assist //
                    dmgsys.CheckForAssist ( player, character, killer );
                }
			}
		}

        private Client GetKiller ( Client player, Client possiblekiller, Character character ) {
            if ( player != possiblekiller && possiblekiller != null && possiblekiller.Exists )
                return possiblekiller;

            Client lasthitter = GetLastHitter ( player, character );
            if ( lasthitter != null )
                return lasthitter;

            return player;
        }

		private static void SpawnAfterDeath ( Client player ) {
            sDeadTimer.Remove ( player, out Timer timer );
			timer.Kill ();
			if ( player.Exists ) {
                Character character = player.GetChar ();
                NAPI.Player.SpawnPlayer ( player, character.Lobby.SpawnPoint, character.Lobby.SpawnRotation.Z );
            }    			
		}

		private void CheckForAssist ( Client player, Character character, Client killer ) {
			if ( AllHitters.ContainsKey ( player ) ) {
				uint halfarmorhp = ( lobby.Armor + lobby.Health ) / 2;
				foreach ( KeyValuePair<Client, int> entry in AllHitters[player] ) {
                    Client target = entry.Key;
					if ( entry.Value >= halfarmorhp ) {
						Character targetcharacter = target.GetChar ();
						if ( target.Exists && targetcharacter.Lobby == character.Lobby && killer != target ) {
                            if ( targetcharacter.Lobby is Arena )
                                targetcharacter.GiveAssist ();
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
				LastHitterDictionary.Remove ( player, out lastHitter );
                if ( lastHitter.Exists ) {
					Character lasthittercharacter = lastHitter.GetChar ();
					if ( character.Lobby == lasthittercharacter.Lobby )
						if ( lasthittercharacter.Lifes > 0 ) {
                            if ( character.Lobby is Arena )
							    lasthittercharacter.GiveKill();
							lastHitter.SendLangNotification ( "got_last_hitted_kill", player.Name );
							AddToKillingSpree ( lastHitter );
						}
				}
			} else
				lastHitter = null;
		}

		public Client GetLastHitter ( Client player, Character character ) {
			if ( LastHitterDictionary.ContainsKey ( player ) ) {
				LastHitterDictionary.Remove ( player, out Client lasthitter );
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
