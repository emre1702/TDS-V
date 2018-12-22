namespace TDS.server.manager.logs {

	partial class ReportsLog {

		public static void Creation ( uint reportuid, uint useruid, string title, string text ) {
			AddReportsLogEntry( reportuid, useruid, title + " | " + text );
		}

		public static void Answer ( uint reportuid, uint useruid, string text ) {
			AddReportsLogEntry( reportuid, useruid, text );
		}

		public static void Remove ( uint reportuid, uint useruid, string text ) {
			AddReportsLogEntry( reportuid, useruid, text );
		}
	}
}
