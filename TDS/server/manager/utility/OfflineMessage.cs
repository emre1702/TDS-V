using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;
using TDS.server.instance.player;
using TDS.server.manager.database;
using TDS.server.manager.player;

namespace TDS.server.manager.utility {

	public class OfflineMessage {
		public string Message;
		public string By;
		public bool FirstTimeLoaded = false;

		public OfflineMessage ( string message, string by ) {
			Message = message;
			if ( uint.TryParse( by, out uint uid ) ) {
				By = Account.GetNameByUID( uid );
			} else
				By = by;
		}
	}

    static class OfflineMessages {

		public static Dictionary<Client, List<OfflineMessage>> PlayerOfflineMessages = new Dictionary<Client, List<OfflineMessage>>();

		public static void AddOfflineMessage ( uint uid, string message, string by ) {
			Database.ExecPrepared( $"INSERT INTO offlinemsg (uid, message, by) VALUES ({uid}, @MESSAGE@, @BY@);", new Dictionary<string, string> {
				{ "@MESSAGE@", message },
				{ "@BY@", by }
			} );
		}

		public static async void LoadOfflineMessages ( Character character ) {
			Client player = character.Player;
			DataTable table = await Database.ExecResult( $"SELECT * FROM offlinemsg WHERE uid = {character.UID};" );
			uint newones = 0;
			PlayerOfflineMessages[player] = new List<OfflineMessage>();
			if ( table.Rows.Count > 0 ) {
				foreach ( DataRow row in table.Rows ) {
					OfflineMessage msg = new OfflineMessage( Convert.ToString( row["message"] ), Convert.ToString( row["by"] ) );
					PlayerOfflineMessages[player].Add( msg );
					if ( Convert.ToSByte( row["loadedonce"] ) == 0 ) {
						++newones;
						msg.FirstTimeLoaded = true;
					}
				}
				if ( newones > 0 ) {
					character.SendLangMessage( "you_got_offline_messages_with_new", PlayerOfflineMessages[player].Count.ToString(), newones.ToString() );
					Database.Exec( $"UPDATE offlinemsg SET loadedonce = 1 WHERE uid = {character.UID} AND loadedonce = 0;" );
				} else
					character.SendLangMessage( "you_got_offline_messages_without_new", PlayerOfflineMessages[player].Count.ToString() );
			}
		}

		[ServerEvent( Event.PlayerDisconnected )]
		public static void ClearOfflineMessagesDictionary ( Client player, DisconnectionType type, string reason ) {
			if ( PlayerOfflineMessages.ContainsKey( player ) )
				PlayerOfflineMessages.Remove( player );
		}
	}
}
