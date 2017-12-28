namespace TDS.server.manager.lobby {

	using GTANetworkAPI;
	using instance.lobby;

	class MainMenu : Script {
		public static Lobby TheLobby;

		public MainMenu () {
            TheLobby = new Lobby ( "mainmenu", new Vector3 ( 0, 0, 999 ), 0 ) {
                DeleteWhenEmpty = false,
                IsOfficial = true,
                spawnPoint = new Vector3 ( 0, 0, 900 )
			};
		}

		public static void Join ( Client player ) {
			TheLobby.AddPlayer ( player, true );
			NAPI.ClientEvent.TriggerClientEvent ( player, "onClientJoinMainMenu" );
		}
	}

}
