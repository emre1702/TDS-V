namespace TDS.server.manager.lobby {

	using GTANetworkAPI;

	static class MapCreatorLobby {
		
		public static void Join ( Client player ) {
            instance.lobby.MapCreateLobby lobby = new instance.lobby.MapCreateLobby ( "MapCreator"+player.Name );
            lobby.SetDefaultSpawnPoint ( new Vector3 ( 198.0562, -935.6811, 30.68681 ) );
            lobby.SetDefaultSpawnRotation ( new Vector3 ( 0, 0, 144.0 ) );
            lobby.AddPlayer ( player );
            lobby.SetPlayerLobbyOwner ( player );
		}
	}

}
