namespace TDS.server.manager.lobby {

	using GTANetworkAPI;
    using TDS.server.instance.player;

    static class GangLobby {

		public static instance.lobby.GangLobby TheLobby;       

		public static async void Create () {
			TheLobby = new instance.lobby.GangLobby ( "gang", 2 ) {
				DeleteWhenEmpty = false,
				IsOfficial = true
			};
            TheLobby.SetDefaultSpawnPoint ( new Vector3 ( 198.0562, -935.6811, 30.68681 ) );
            TheLobby.SetDefaultSpawnRotation ( new Vector3 ( 0, 0, 144.0 ) );

			await TheLobby.ActivateBansSaving();
		}

		public static void Join ( Character character ) {
			TheLobby.AddPlayer ( character );
		}
	}

}
