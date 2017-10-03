using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Threading.Tasks;
using System;

namespace Manager {
	class MinuteTimer : Script {

		public MinuteTimer ( ) {
			Class.Timer.SetTimer ( this.MinuteTimerFunc, 60 * 1000, -1 );
		}

		private async void MinuteTimerFunc ( ) {
			try {

				// playtime //
				List<Client> players = API.getAllPlayers ();
				for ( int i = 0; i < players.Count; i++ ) {
					if ( players[i].exists ) {
						Class.Character character = players[i].GetChar ();
						if ( character.loggedIn ) {
							character.playtime++;
							if ( character.playtime % 30 == 0 ) {
								await Account.SavePlayerData ( players[i] ).ConfigureAwait ( false );
							}
						}
					}
				}

				// log-save //
				await Log.SaveInDatabase ().ConfigureAwait ( false );
			} catch ( Exception ex ) {
				API.consoleOutput ( "Error in MinuteTimerFunc:" + ex.Message );
			}
		}

	}
}
