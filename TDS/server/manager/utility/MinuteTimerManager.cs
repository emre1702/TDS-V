using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Threading.Tasks;

namespace Manager {
	class MinuteTimer {

		public static void MinuteTimerOnStart ( ) {
			Class.Timer.SetTimer ( MinuteTimerFunc, 60 * 1000, -1 );
		}

		private static async void MinuteTimerFunc ( ) {

			// playtime //
			List<Client> players = API.shared.getAllPlayers ();
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
		}

	}
}
