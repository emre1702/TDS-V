namespace TDS.server.manager.logs {

	using GTANetworkAPI;
	using player;

	partial class Log {

		public static void LobbyOwner ( string command, Client player, Client target, ref string lobbyname ) {
			AddLogEntry ( "lobbyowner", command, lobbyname, player.SocialClubName, target != null ? Account.PlayerUIDs[target.SocialClubName].ToString () : "0" );
		}

		public static void LobbyJoin ( Client player, ref string oldlobbyname, ref string lobbyname ) {
			AddLogEntry ( "lobbyjoin", oldlobbyname, lobbyname, player.SocialClubName );
		}
	}

}
