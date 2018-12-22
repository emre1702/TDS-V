namespace TDS.server.manager.logs {

	using GTANetworkAPI;
	using System.Collections.Generic;
	using TDS.server.enums;
	using TDS.server.instance.player;
	using TDS.server.manager.database;
	using TDS.server.manager.utility;

	partial class AdminLog {

		public static void Log ( AdminLogType type, uint adminuid, uint targetuid, string info, int time = 0, int lobbyid = 0 ) {
			AddAdminLogEntry ( type, adminuid, targetuid, info );

			switch ( type ) {
				case AdminLogType.PERMABAN:
					Database.ExecPrepared ( $"INSERT INTO banhistory (uid, shouldtime, reason, date) VALUES ({targetuid}, -1, @reason@, '{Utility.GetTimestamp ()}');", new Dictionary<string, string> {
						{ "@reason@", info }
					} );
					break;
				case AdminLogType.TIMEBAN:
					Database.ExecPrepared ( $"INSERT INTO banhistory (uid, shouldtime, reason, date) VALUES ({targetuid}, {time}, @reason@, '{Utility.GetTimestamp ()}');", new Dictionary<string, string> {
						{ "@reason@", info }
					} );
					break;
				case AdminLogType.UNBAN:
					Database.ExecPrepared ( $"INSERT INTO banhistory (uid, shouldtime, reason, date) VALUES ({targetuid}, 0, @reason@, '{Utility.GetTimestamp ()}');", new Dictionary<string, string> {
						{ "@reason@", info }
					} );
					break;

				case AdminLogType.PERMABANLOBBY:
					Database.ExecPrepared( $"INSERT INTO lobbybanhistory (uid, lobbyid, shouldtime, reason, date) VALUES ({targetuid}, {lobbyid}, - 1, @reason@, '{Utility.GetTimestamp()}');", new Dictionary<string, string> {
						{ "@reason@", info }
					} );
					break;
				case AdminLogType.TIMEBANLOBBY:
					Database.ExecPrepared( $"INSERT INTO lobbybanhistory (uid, lobbyid, shouldtime, reason, date) VALUES ({targetuid}, {lobbyid}, {time}, @reason@, '{Utility.GetTimestamp()}');", new Dictionary<string, string> {
						{ "@reason@", info }
					} );
					break;
				case AdminLogType.UNBANLOBBY:
					Database.ExecPrepared( $"INSERT INTO lobbybanhistory (uid, lobbyid, shouldtime, reason, date) VALUES ({targetuid}, {lobbyid}, 0, @reason@, '{Utility.GetTimestamp()}');", new Dictionary<string, string> {
						{ "@reason@", info }
					} );
					break;

			}
		}
	}

}
