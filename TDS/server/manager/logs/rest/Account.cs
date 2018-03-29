using GTANetworkAPI;
using TDS.server.enums;

namespace TDS.server.manager.logs {

	partial class Log {

		public static void Login ( Client player ) {
			AddLogEntry ( LogType.LOGIN, "Serial: "+player.Serial+" | IP: "+player.Address, "Account", player.SocialClubName );
		}

		public static void Register ( Client player ) {
			AddLogEntry ( LogType.REGISTER, "Serial: " + player.Serial + " | IP: " + player.Address, "Account", player.SocialClubName );
		}
	}

}
