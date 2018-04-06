namespace TDS.server.manager.lobby {

	using GTANetworkAPI;
    using TDS.server.instance.player;

    static class MapCreatorLobby {
		
		public static void Join ( Character character ) {
            instance.lobby.MapCreateLobby lobby = new instance.lobby.MapCreateLobby ( "MapCreator"+ character.Player.Name );
            lobby.SetDefaultSpawnPoint ( new Vector3 ( 198.0562, -935.6811, 30.68681 ) );
            lobby.SetDefaultSpawnRotation ( new Vector3 ( 0, 0, 144.0 ) );
			lobby.BanAllowed = false;
            lobby.AddPlayer ( character );
            lobby.SetPlayerLobbyOwner ( character );
		}
	}

}
