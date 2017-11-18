namespace TDS.server.manager.logs {

	partial class Log {

		public static void Error ( string errorstr, string lobbyname = "DEFAULT" ) {
			AddLogEntry ( "error", errorstr, lobbyname );
		}
	}

}
