using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	partial class Log {
		public static void Admin ( string command, Client player, string targetuid, string lobbyname ) {
			AddLogEntry ( "admin", command, lobbyname, player.socialClubName, targetuid );
		}

		public static void Admin ( string command, Client player, Client target, string lobbyname ) {
			Admin ( command, player, target?.socialClubName, lobbyname );
		}
	}
}