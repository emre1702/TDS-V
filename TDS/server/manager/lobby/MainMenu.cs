namespace TDS.server.manager.lobby {

	using GTANetworkAPI;
	using instance.lobby;

	class MainMenu : Script {
		public static Lobby TheLobby;

		public MainMenu () {
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
