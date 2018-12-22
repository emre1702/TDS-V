using GTANetworkAPI;
using TDS.server.enums;

namespace TDS.server.manager.logs {

	partial class Log {

		public static void Error ( string errorstr, string lobbyname = "DEFAULT" ) {
            NAPI.Util.ConsoleOutput ( "[ERROR IN "+lobbyname + "] " + errorstr );
			AddLogEntry ( LogType.ERROR, errorstr, lobbyname );
		}
	}

}
