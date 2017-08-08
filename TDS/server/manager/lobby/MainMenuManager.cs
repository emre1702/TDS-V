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
			API.shared.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_IN, 100 );
			lobby.AddPlayer ( player, true );
			player.triggerEvent ( "onClientJoinMainMenu" );
		}
	}
}