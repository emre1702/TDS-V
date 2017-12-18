namespace TDS.server.manager.lobby {

	using GTANetworkAPI;
	using instance.lobby;

	class GangLobby : Script {

		public static Lobby TheLobby;

		public GangLobby () {
			/*TheLobby = new Lobby ( "gang", 2, false ) {
				DeleteWhenEmpty = false,
				IsOfficial = true
			};
			TheLobby.AddSpawnPoint ( new Vector3 ( 8.699318, -2.050943, 70.29607 ), new Vector3 ( 0, 0, 65.79314 ) );
			TheLobby.Start ();*/
		}

		public static void Join ( Client player ) {
			TheLobby.AddPlayer ( player, true );
		}
	}

}
