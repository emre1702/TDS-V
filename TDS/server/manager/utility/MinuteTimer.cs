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
								Account.SavePlayerData ( player );
							}
						}
					}
				}

				// log-save //
				Log.SaveInDatabase ();
                Season.SaveSeason ();
			} catch ( Exception ex ) {
				Log.Error ( "Error in MinuteTimerFunc:" + ex.Message );
			}
		}

	}

}
