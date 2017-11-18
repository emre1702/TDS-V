namespace TDS.server.manager.logs {

	using GTANetworkAPI;

	partial class Log {
		public static void Admin ( string command, Client player, string targetuid, string lobbyname ) {
			AddLogEntry ( "admin", command, lobbyname, player.SocialClubName, targetuid );
		}

		public static void Admin ( string command, Client player, Client target, string lobbyname ) {
			Admin ( command, player, target?.SocialClubName, lobbyname );
		}
	}

}
