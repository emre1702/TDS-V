﻿namespace TDS.server.manager.lobby {

	using GTANetworkAPI;
	using instance.lobby;

	static class MapCreateLobby {
		public static Lobby TheLobby;

		public static void Create () {
			/*TheLobby = new Lobby ( "mapcreate", 3, false, false, true ) {
				DeleteWhenEmpty = false,
				IsOfficial = true,
				IsMapCreateLobby = true
			};
			TheLobby.AddSpawnPoint ( new Vector3 ( 8.699318, -2.050943, 70.29607 ), new Vector3 ( 0, 0, 65.79314 ) );
			TheLobby.Start ();*/
		}

		public static void Join ( Client player ) {
			TheLobby.AddPlayer ( player, true );
		}
	}

}
