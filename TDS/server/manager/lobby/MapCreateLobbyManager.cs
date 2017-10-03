using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace Manager {
	static class MapCreateLobby {
		public static Class.Lobby lobby;

		public static void Create ( ) {
			lobby = new Class.Lobby ( "mapcreate", 3, false, false );
			lobby.deleteWhenEmpty = false;
		}

		public static void Join ( Client player ) {
			lobby.AddPlayer ( player, true );
		}
	}
}