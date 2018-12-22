namespace TDS.server.manager.logs {

	using System.Collections.Generic;
    using System.Text;
    using database;
	using player;
	using TDS.server.enums;
	using utility;

	partial class Log {

        private static List<LogEntry> entries = new List<LogEntry> ();
        private static Dictionary<string, string> logQueryParameters = new Dictionary<string, string> ();
        private static StringBuilder builder = new StringBuilder ();

        class LogEntry {
            public int Index;

            public string UID;
            public string TargetUID;
            public LogType Type;
            public string Info;
            public string Lobby;
            public string Date;

            public LogEntry() {
                entries.Add ( this );
            }

            public void GetValueString () {
                builder.Append ( $"({UID}, {TargetUID}, '{(int)Type}', '@info{Index}@', '{Lobby}', '{Date}')" );
                logQueryParameters[$"@info{Index}@"] = Info;
            }
        }

		private static void AddLogEntry ( LogType type, string info, string lobby = "DEFAULT", string playername = "-", string targetUID = "0" ) {
            new LogEntry {
                Index = entries.Count,
                UID = playername != "-" ? Account.PlayerUIDs[playername].ToString () : "0",
                TargetUID = targetUID,
                Type = type,
                Info = playername != "-" ? info : "["+playername+"]"+info,
                Lobby = lobby,
                Date = Utility.GetTimestamp()
            };
        }

		public static void SaveInDatabase () {
			int amount = entries.Count;
			if ( amount > 0 ) {
                builder.Append ( "INSERT INTO log (uid, targetuid, type, info, lobby, date) VALUES " );
                entries[0].GetValueString ();
				for ( int i = 1; i < entries.Count; i++ ) {
                    builder.Append ( ", " );
                    entries[i].GetValueString ();
				}

				Database.ExecPrepared ( builder.ToString(), logQueryParameters );
                entries.Clear ();
                logQueryParameters.Clear ();
                builder.Clear ();
            }
		}
	}

}
