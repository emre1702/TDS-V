using GTANetworkAPI;

namespace TDS.server.manager.logs {

	partial class Log {

		public static void Error ( string errorstr, string lobbyname = "DEFAULT" ) {
            NAPI.Util.ConsoleOutput ( "[ERROR IN "+lobbyname + "] " + errorstr );
			AddLogEntry ( "error", errorstr, lobbyname );
		}
	}

}
