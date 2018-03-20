namespace TDS.server.manager.logs {

	using GTANetworkAPI;
	using player;

	partial class Log {

		public static void LobbyOwner ( string command, Client player, Client target, string lobbyname ) {
			AddLogEntry ( "lobbyowner", command, lobbyname, player.SocialClubName, target != null ? Account.PlayerUIDs[target.SocialClubName].ToString () : "0" );
		}

		public static void LobbyJoin ( Client player, string oldlobbyname, string lobbyname ) {
			AddLogEntry ( "lobbyjoin", oldlobbyname, lobbyname, player.SocialClubName );
		}
	}

}
