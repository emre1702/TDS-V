namespace TDS.server.manager.lobby {

	using GTANetworkAPI;
	using instance.lobby;

	static class MainMenu {
		public static Lobby TheLobby;

		public static void Create() {
            TheLobby = new Lobby ( "mainmenu", 0 ) {
                DeleteWhenEmpty = false,
                IsOfficial = true,
                AroundSpawnPoint = 5f
			};
		}

		public static void Join ( Client player ) {
			TheLobby.AddPlayer ( player, true );
			NAPI.ClientEvent.TriggerClientEvent ( player, "onClientJoinMainMenu" );
		}
	}

}
