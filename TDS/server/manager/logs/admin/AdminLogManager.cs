namespace TDS.server.manager.logs {

	using System.Collections.Generic;
	using System.Text;
	using database;
	using player;
	using TDS.server.enums;
	using utility;

	partial class AdminLog {

		private static List<AdminLogEntry> entries = new List<AdminLogEntry> ();
		private static Dictionary<string, string> logQueryParameters = new Dictionary<string, string> ();
		private static StringBuilder builder = new StringBuilder ();

		class AdminLogEntry {
			public int Index;

			public uint UID;
			public uint TargetUID;
			public AdminLogType Type;
			public string Info;
			public string Date;

			public AdminLogEntry ( ) {
				entries.Add ( this );
			}

			public void GetValueString ( ) {
				builder.Append ( $"({UID}, {TargetUID}, '{(int) Type}', '@info{Index}@', '{Date}')" );
				logQueryParameters[$"@info{Index}@"] = Info;
			}
		}

		private static void AddAdminLogEntry ( AdminLogType type, uint playerUID, uint targetUID, string info ) {
			new AdminLogEntry {
				Index = entries.Count,
				UID = playerUID,
				TargetUID = targetUID,
				Type = type,
				Info = info,
				Date = Utility.GetTimestamp ()
			};
		}

		public static void SaveInDatabase ( ) {
			int amount = entries.Count;
			if ( amount > 0 ) {
				builder.Append ( "INSERT INTO adminlog (adminuid, targetuid, type, info, date) VALUES " );
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
