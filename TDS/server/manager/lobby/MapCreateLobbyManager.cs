using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace Manager {
	static class MapCreateLobby {
		public static Class.Lobby lobby;

		public static void Create ( ) {
			lobby = new Class.Lobby ( "mapcreate", 3, false, false, true );
			lobby.deleteWhenEmpty = false;
			lobby.isOfficial = true;
			lobby.AddSpawnPoint ( new Vector3 ( 8.699318, -2.050943, 70.29607 ), new Vector3 ( 0, 0, 65.79314 ) );
			lobby.Start ();
		}

		public static void Join ( Client player ) {
			lobby.AddPlayer ( player, true );
		}
	}
}