namespace TDS.server.manager.utility {

	using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;
    using TDS.server.instance.player;

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
		public static Dictionary<uint, List<Character>> AdminsOnline = new Dictionary<uint, List<Character>> {
			[1] = new List<Character> (),
			[2] = new List<Character> (),
			[3] = new List<Character> (),
			[4] = new List<Character> ()
		};
		private const int adminMaxLvl = 4;

		public static void SetOnline ( Character character ) {
			if ( adminMaxLvl >= character.AdminLvl ) {
				AdminsOnline[character.AdminLvl].Add ( character );
			}
		}

		public static void SetOffline ( Character character ) {
			if ( adminMaxLvl >= character.AdminLvl ) {
				AdminsOnline[character.AdminLvl].Remove ( character );
			}
		}

		public static void SendChatMessageToAdmins ( string message ) {
			for ( uint adminlvl = 1; adminlvl <= adminMaxLvl; adminlvl++ ) {
				for ( int j = 0; j < AdminsOnline[adminlvl].Count; j++ ) {
					Character character = AdminsOnline[adminlvl][j];
                    if ( character.Player.Exists )
                        NAPI.Chat.SendChatMessageToPlayer ( character.Player, message );
				}
			}
		}
	}

}
