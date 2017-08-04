using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	partial class Log {
		public static void VIP ( string command, Client player, Client target, string lobbyname ) {
			AddLogEntry ( "vip", command, lobbyname, Account.playerUIDs[player.socialClubName].ToString (), target != null ? Account.playerUIDs[target.socialClubName].ToString () : "0" );
		}
	}
}