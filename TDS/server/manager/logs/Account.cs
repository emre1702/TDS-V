using GTANetworkAPI;

namespace TDS.server.manager.logs {

	partial class Log {

		public static void Login ( Client player ) {
			AddLogEntry ( "login", "Serial: "+player.Serial+" | IP: "+player.Address, "Account", player.SocialClubName );
		}

		public static void Register ( Client player ) {
			AddLogEntry ( "register", "Serial: " + player.Serial + " | IP: " + player.Address, "Account", player.SocialClubName );
		}
	}

}
