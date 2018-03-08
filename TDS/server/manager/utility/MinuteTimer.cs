namespace TDS.server.manager.utility {

	using System;
	using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;
	using instance.player;
	using instance.utility;
	using logs;

	class MinuteTimer : Script {

        private static int counter = 0;
		private const int savePlayerAfterTick = 20 * 60 * 1000;

		public MinuteTimer () {
			Timer.SetTimer ( MinuteTimerFunc, 60 * 1000, 0 );
		}

		private void MinuteTimerFunc () {
			try {
				// playtime //
				int currenttick = Environment.TickCount;
				List<Client> players = NAPI.Pools.GetAllPlayers ();
				foreach ( Client player in players ) {
					if ( player.Exists ) {
						Character character = player.GetChar ();
						if ( character.LoggedIn ) {
							++character.Playtime;
							if ( ( currenttick - character.StartTick ) / savePlayerAfterTick > character.LastSave ) {
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
