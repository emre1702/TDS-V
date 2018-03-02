namespace TDS.server.manager.utility {

	using System;
	using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;
	using instance.player;
	using instance.utility;
	using logs;
	using player;

	class MinuteTimer : Script {

        private static int counter = 0;

		public MinuteTimer () {
			Timer.SetTimer ( MinuteTimerFunc, 60 * 1000, -1 );
		}

		private void MinuteTimerFunc () {
			try {
				// playtime //
				List<Client> players = NAPI.Pools.GetAllPlayers ();
				foreach ( Client player in players ) {
					if ( player.Exists ) {
						Character character = player.GetChar ();
						if ( character.LoggedIn ) {
							character.Playtime++;
							if ( character.Playtime % 30 == 0 ) {
                                character.SaveData ();
							}
						}
					}
				}

                ++counter;

                // log-save //
                Log.SaveInDatabase ();

                if ( counter % 30 == 0 )
                    Season.SaveSeason ();
			} catch ( Exception ex ) {
				Log.Error ( ex.ToString() );
			}
		}

	}

}
