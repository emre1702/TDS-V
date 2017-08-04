using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	partial class Log {
		public static void Chat ( string chatstr, Client player, string lobbyname ) {
			AddLogEntry ( "chat", chatstr, player.socialClubName+": "+lobbyname, Account.playerUIDs[player.socialClubName].ToString() );
		}
	}
}