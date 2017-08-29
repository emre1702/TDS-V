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
		public static ConcurrentDictionary<int,ConcurrentBag<Client>> adminsOnline = new ConcurrentDictionary<int, ConcurrentBag<Client>> {
			[1] = new ConcurrentBag<Client>(),
			[2] = new ConcurrentBag<Client>(),
			[3] = new ConcurrentBag<Client>(),
			[4] = new ConcurrentBag<Client>()
		};
		private static int adminMaxLvl = 4;

		public static void SetOnline ( Client player, int adminlvl = 0 ) {
			int alvl = adminlvl == 0 ? player.GetChar ().adminLvl : adminlvl;
			if ( adminMaxLvl >= alvl ) {
				adminsOnline[alvl].Add ( player );
			}
		}

		public static void SetOffline ( Client player, int adminlvl = 0 ) {
			int alvl = adminlvl == 0 ? player.GetChar ().adminLvl : adminlvl;
			if ( adminMaxLvl >= alvl ) {
				adminsOnline[alvl].TryTake ( out player );
			}
		}

		public static void SendChatMessageToAdmins ( string message ) {
			for ( int i = 1; i <= adminMaxLvl; i++ ) {
				for ( int j = 0; j < adminsOnline[i].Count; j++ ) {
					adminsOnline[i].TryPeek ( out Client player );
					if ( player.exists )
						player.sendChatMessage ( message );
				}
			}
		}
	} 
}