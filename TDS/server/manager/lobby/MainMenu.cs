namespace TDS.server.manager.lobby {

	using GTANetworkAPI;
	using instance.lobby;

	class MainMenu : Script {
		public static Lobby TheLobby;

		public MainMenu () {
			TheLobby = new Lobby ( "mainmenu", 0, false, false ) {
				DeleteWhenEmpty = false
			};
		}

		public static void Join ( Client player ) {
			TheLobby.AddPlayer ( player, true );
			player.TriggerEvent ( "onClientJoinMainMenu" );
		}
	}

}
