namespace TDS.server.manager.lobby {

	using GTANetworkAPI;

	static class MapCreatorLobby {
		
		public static void Join ( Client player ) {
            instance.lobby.MapCreateLobby lobby = new instance.lobby.MapCreateLobby ( "MapCreator"+player.Name );
            lobby.SetDefaultSpawnPoint ( new Vector3 ( 8.699318, 2.050943, 70.29607 ), 5 );
            lobby.SetDefaultSpawnRotation ( new Vector3 ( 0, 0, 65.79314 ) );
            lobby.AddPlayer ( player );
            lobby.SetPlayerLobbyOwner ( player );
            player.Freeze ( false );
		}
	}

}
