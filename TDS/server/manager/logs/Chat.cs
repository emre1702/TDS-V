namespace TDS.server.manager.logs {

	using GTANetworkAPI;

	partial class Log {
		public static void Chat ( string chatstr, Client player, string lobbyname, uint targetUID = 0 ) {
			AddLogEntry ( "chat", chatstr, lobbyname, player.SocialClubName, targetUID: targetUID.ToString() );
		}
	}

}
