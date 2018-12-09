using GTANetworkAPI;
using System.Collections.Generic;

namespace TDS.server.instance.lobby.ganglobby {

	partial class Gang {

		/*public void SendAllOnlinePlayerLangMessage ( string type, params string[] args ) {
			Dictionary<Language, string> lang = ServerLanguage.GetLangDictionary( type, args );
			foreach ( Character character in membersOnline ) {
				NAPI.Chat.SendChatMessageToPlayer( character.Player, lang[character.Language] );
			}		
		}

		public void SendAllOnlinePlayerLangNotification ( string type, params string[] args ) {
			Dictionary<Language, string> lang = ServerLanguage.GetLangDictionary( type, args );
			foreach ( Character character in membersOnline ) {
				NAPI.Notification.SendNotificationToPlayer( character.Player, lang[character.Language] );
			}
		}

		public void SendAllPlayerLangMessage ( string type, params string[] args ) {
			Dictionary<Language, string> lang = ServerLanguage.GetLangDictionary( type, args );
			HashSet<uint> membersdone = new HashSet<uint>();

			foreach ( Character character in membersOnline ) {
				NAPI.Chat.SendChatMessageToPlayer( character.Player, lang[character.Language] );
				membersdone.Add( character.UID );
			}

			foreach ( KeyValuePair<uint, uint> entry in membersRank ) {
				if ( !membersdone.Contains( entry.Key ) ) {
					OfflineMessages.AddOfflineMessage( entry.Key, lang[Language.ENGLISH], "Gang" );		
				}
			}
		}*/

	}
}
