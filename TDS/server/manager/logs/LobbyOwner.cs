using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	partial class Log {
		public static void LobbyOwner ( string command, Client player, Client target, string lobbyname ) {
			AddLogEntry ( "lobbyowner", command, lobbyname, Account.playerUIDs[player.socialClubName].ToString (), target != null ? Account.playerUIDs[target.socialClubName].ToString () : "0" );
		}
	}
}
