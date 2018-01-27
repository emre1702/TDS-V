namespace TDS.server.manager.utility {

	using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;

	static class Admin {

		public static Dictionary<uint, string> NameByLevel = new Dictionary<uint, string> {
			[0] = "User",
			[1] = "Supporter",
			[2] = "Administrator",
			[3] = "Project manager",
			[4] = "Owner"
		};
		public static Dictionary<uint, string> LevelFontColor = new Dictionary<uint, string> {
			[0] = "#s#",
			[1] = "#g#",
			[2] = "#o#",
			[3] = "#dr#",
			[4] = "#dr#"
        };
		public static Dictionary<uint, List<Client>> AdminsOnline = new Dictionary<uint, List<Client>> {
			[1] = new List<Client> (),
			[2] = new List<Client> (),
			[3] = new List<Client> (),
			[4] = new List<Client> ()
		};
		private const int adminMaxLvl = 4;

		public static void SetOnline ( Client player, uint adminlvl = 0 ) {
			uint alvl = adminlvl == 0 ? player.GetChar ().AdminLvl : adminlvl;
			if ( adminMaxLvl >= alvl ) {
				AdminsOnline[alvl].Add ( player );
			}
		}

		public static void SetOffline ( Client player, uint adminlvl = 0 ) {
			uint alvl = adminlvl == 0 ? player.GetChar ().AdminLvl : adminlvl;
			if ( adminMaxLvl >= alvl ) {
				AdminsOnline[alvl].Remove ( player );
			}
		}

		public static void SendChatMessageToAdmins ( string message ) {
			for ( uint adminlvl = 1; adminlvl <= adminMaxLvl; adminlvl++ ) {
				for ( int j = 0; j < AdminsOnline[adminlvl].Count; j++ ) {
					Client player = AdminsOnline[adminlvl][j];
					if ( player.Exists )
						player.SendChatMessage ( message );
				}
			}
		}
	}

}
