using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;

namespace Manager {
	static class MainMenu {
		public static Class.Lobby lobby;

		public static void Create ( ) {
			lobby = new Class.Lobby ( "mainmenu", 0, false, false );
			lobby.deleteWhenEmpty = false;
		}

		public static void Join ( Client player ) {
			lobby.AddPlayer ( player, true );
			player.triggerEvent ( "onClientJoinMainMenu" );
		}
	}
}