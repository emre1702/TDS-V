namespace TDS.server.manager.lobby {

	using GTANetworkAPI;
	using instance.lobby;
    using TDS.server.instance.player;

    static class MainMenu {
		public static Lobby TheLobby;

		public static void Create() {
            TheLobby = new Lobby ( "mainmenu", 0 ) {
                DeleteWhenEmpty = false,
                IsOfficial = true,
                AroundSpawnPoint = 5f
			};
		}

		public static void Join ( Character character ) {
			TheLobby.AddPlayer ( character, true );
		}
	}

}
