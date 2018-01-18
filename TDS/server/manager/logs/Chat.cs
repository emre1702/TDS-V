namespace TDS.server.manager.logs {

	using GTANetworkAPI;

	partial class Log {
		public static void Chat ( string chatstr, Client player, string lobbyname ) {
			AddLogEntry ( "chat", chatstr, lobbyname, player.SocialClubName );
		}
	}

}
