namespace TDS.server.manager.logs {

	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using database;
	using player;
	using utility;

	partial class Log {

		private static List<string> logQueryUIDs = new List<string> ();
		private static List<string> logQueryNames = new List<string> ();
		private static List<string> logQueryTargetUIDs = new List<string> ();
		private static List<string> logQueryTypes = new List<string> ();
		private static List<string> logQueryInfos = new List<string> ();
		private static List<string> logQueryLobbies = new List<string> ();
		private static List<string> logQueryDates = new List<string> ();

		private static Dictionary<string, string> logQueryParameters = new Dictionary<string, string> ();

		private static void AddLogEntry ( string type, string info, string lobby = "DEFAULT", string playername = "-", string targetUID = "0" ) {
			logQueryUIDs.Add ( playername != "-" ? Account.PlayerUIDs[playername].ToString () : "0" );
			logQueryNames.Add ( playername );
			logQueryTargetUIDs.Add ( targetUID );
			logQueryTypes.Add ( type );
			logQueryInfos.Add ( info );
			logQueryLobbies.Add ( lobby );
			logQueryDates.Add ( Utility.GetTimestamp () );
		}

		private static string GetValueString ( int index ) {
			string str = "(@uid" + index + "@, @name" + index + "@, @targetuid" + index + "@, @type" + index + "@, @info" + index + "@, @lobby" + index + "@, @date" + index + "@)";
			logQueryParameters["@uid" + index + "@"] = logQueryUIDs[index];
			logQueryParameters["@name" + index + "@"] = logQueryNames[index];
			logQueryParameters["@targetuid" + index + "@"] = logQueryTargetUIDs[index];
			logQueryParameters["@type" + index + "@"] = logQueryTypes[index];
			logQueryParameters["@info" + index + "@"] = logQueryInfos[index];
			logQueryParameters["@lobby" + index + "@"] = logQueryLobbies[index];
			logQueryParameters["@date" + index + "@"] = logQueryDates[index];
			return str;
		}

		private static void ResetLists () {
			logQueryUIDs = new List<string> ();
			logQueryNames = new List<string> ();
			logQueryTargetUIDs = new List<string> ();
			logQueryTypes = new List<string> ();
			logQueryInfos = new List<string> ();
			logQueryLobbies = new List<string> ();
			logQueryDates = new List<string> ();
		}

		public static void SaveInDatabase () {
			int amount = logQueryUIDs.Count;
			if ( amount > 0 ) {
				logQueryParameters = new Dictionary<string, string> ();
				string sql = "INSERT INTO log (uid, name, targetuid, type, info, lobby, date) VALUES " + GetValueString ( 0 );

				for ( int i = 1; i < logQueryUIDs.Count; i++ ) {
					try {
						sql += ", " + GetValueString ( i );
					} catch ( Exception e ) {
						Error ( "Error in SaveInDatabase: " + e );
					}
				}

				Database.ExecPrepared ( sql, logQueryParameters );
				ResetLists ();
			}
		}
	}

}
