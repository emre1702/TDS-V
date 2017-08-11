using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Threading.Tasks;

namespace Manager {
	class MinuteTimer {

		public static void MinuteTimerOnStart ( ) {
			Class.Timer.SetTimer ( () => Task.Run ( ( ) => MinuteTimerFunc() ), 60 * 1000, -1 );
		}

		private static void MinuteTimerFunc ( ) {

			// playtime //
			List<Client> players = API.shared.getAllPlayers ();
			for ( int i = 0; i < players.Count; i++ ) {
				Class.Character character = players[i].GetChar ();
				if ( character.loggedIn ) {
					character.playtime++;
					if ( character.playtime % 30 == 0 ) {
						Account.SavePlayerData ( players[i] );
					}
				}
			}

			// log-save //
			Log.SaveInDatabase ();
		}

	}
}
