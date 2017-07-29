﻿using System.Collections.Generic;

namespace Manager {
	partial class Log {

		private static List<string> logQueryUIDs = new List<string> ();
		private static List<string> logQueryTargetUIDs = new List<string> ();
		private static List<string> logQueryTypes = new List<string> ();
		private static List<string> logQueryInfos = new List<string> ();
		private static List<string> logQueryLobbies = new List<string> ();

		private static Dictionary<string, string> logQueryParameters = new Dictionary<string, string> ();

		private static void AddLogEntry ( string type, string info, string lobby = "DEFAULT", string UID = "DEFAULT", string targetUID = "DEFAULT" ) {
			logQueryUIDs.Add ( UID );
			logQueryTargetUIDs.Add ( targetUID );
			logQueryTypes.Add ( type );
			logQueryInfos.Add ( info );
			logQueryLobbies.Add ( lobby );
		}

		private static string GetValueString ( int index ) {
			string str = "(@UID"+index+"@, @targetUID"+index+"@, @type"+index+"@, @info"+index+"@, @lobby"+index+"@)";
			logQueryParameters["@UID" + index + "@"] = logQueryUIDs[index];
			logQueryParameters["@targetUID" + index + "@"] = logQueryTargetUIDs[index];
			logQueryParameters["@type" + index + "@"] = logQueryTypes[index];
			logQueryParameters["@info" + index + "@"] = logQueryInfos[index];
			logQueryParameters["@lobby" + index + "@"] = logQueryLobbies[index];
			return str;
		}

		private static void ResetLists ( ) {
			logQueryUIDs = new List<string> ();
			logQueryTargetUIDs = new List<string> ();
			logQueryTypes = new List<string> ();
			logQueryInfos = new List<string> ();
			logQueryLobbies = new List<string> ();
		}

		public static void SaveInDatabase ( ) {
			int amount = logQueryUIDs.Count;
			if ( amount > 0 ) {
				logQueryParameters = new Dictionary<string, string> ();
				string sql = "INSERT INTO log (UID, targetUID, type, info, lobby) VALUES "+ GetValueString ( 0 );

				for ( int i = 1; i < logQueryUIDs.Count; i++ ) {
					try {
						sql += ", " + GetValueString ( i );
					} catch ( System.Exception e ) {
						Error ( "Error in SaveInDatabase: "+e.ToString() );
					}
				}

				Database.ExecPrepared ( sql, logQueryParameters );
				ResetLists ();
			}
		}
	}
}