namespace TDS.server.manager.logs {

	using System.Collections.Generic;
	using System.Text;
	using database;
	using player;
	using TDS.server.enums;
	using utility;

	partial class ReportsLog {

		private static List<ReportsLogEntry> entries = new List<ReportsLogEntry> ();
		private static Dictionary<string, string> logQueryParameters = new Dictionary<string, string> ();
		private static StringBuilder builder = new StringBuilder ();

		class ReportsLogEntry {
			public int Index;

			public uint ReportUID;
			public uint UserUID;
			public string Info;
			public string Date;

			public ReportsLogEntry ( ) {
				entries.Add ( this );
			}

			public void GetValueString ( ) {
				builder.Append ( $"({ReportUID}, {UserUID}, '@info{Index}@', '{Date}')" );
				logQueryParameters[$"@info{Index}@"] = Info;
			}
		}

		private static void AddReportsLogEntry ( uint reportUID, uint userUID, string info ) {
			new ReportsLogEntry {
				Index = entries.Count,
				ReportUID = reportUID,
				UserUID = userUID,
				Info = info,
				Date = Utility.GetTimestamp ()
			};
		}

		public static void SaveInDatabase ( ) {
			int amount = entries.Count;
			if ( amount > 0 ) {
				builder.Append ( "INSERT INTO reportslog (reportuid, useruid, info, date) VALUES " );
				entries[0].GetValueString ();
				for ( int i = 1; i < entries.Count; i++ ) {
					builder.Append ( ", " );
					entries[i].GetValueString ();
				}

				Database.ExecPrepared ( builder.ToString (), logQueryParameters );
				entries.Clear ();
				logQueryParameters.Clear ();
				builder.Clear ();
			}
		}
	}

}
