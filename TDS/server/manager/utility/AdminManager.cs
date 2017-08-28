using System.Collections.Concurrent;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	static class Admin {

		public static ConcurrentDictionary<int, string> nameByLevel = new ConcurrentDictionary<int, string> {
			[0] = "User",
			[1] = "Supporter",
			[2] = "Administrator",
			[3] = "Project manager",
			[4] = "Owner"
		};
		public static ConcurrentDictionary<int, string> levelFontColor = new ConcurrentDictionary<int, string> {
			[0] = "~s~",
			[1] = "~g~",
			[2] = "~o~",
			[3] = "~dr~",
			[4] = "~dr~"
		};
		public static List<Client>[] adminsOnline = new List<Client>[] { new List<Client>(), new List<Client> (), new List<Client> (), new List<Client>() };

		public static void SetOnline ( Client player, int adminlvl = 0 ) {
			int alvl = adminlvl == 0 ? player.GetChar ().adminLvl : adminlvl;
			if ( adminsOnline.Length >= alvl ) {
				adminsOnline[alvl - 1].Add ( player );
			}
		}

		public static void SetOffline ( Client player, int adminlvl = 0 ) {
			int alvl = adminlvl == 0 ? player.GetChar ().adminLvl : adminlvl;
			if ( adminsOnline.Length >= alvl ) {
				adminsOnline[alvl - 1].Remove ( player );
			}
		}

		public static void SendChatMessageToAdmins ( string message ) {
			for ( int i = 0; i < adminsOnline.Length; i++ ) {
				for ( int j = 0; j < adminsOnline[i].Count; j++ ) {
					Client player = adminsOnline[i][j];
					if ( player.exists )
						player.sendChatMessage ( message );
				}
			}
		}
	} 
}