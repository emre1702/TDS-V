using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	class MinuteTimerManager : GrandTheftMultiplayer.Server.API.Script {

		public MinuteTimerManager ( ) {
			Class.Timer.SetTimer ( this.MinuteTimerFunc, 60 * 1000, -1 );
		}

		private void MinuteTimerFunc ( ) {

			// playtime //
			List<Client> players = API.getAllPlayers ();
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
