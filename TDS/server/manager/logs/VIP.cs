namespace TDS.server.manager.logs {

	using GTANetworkAPI;
	using player;

	partial class Log {
		public static void VIP ( string command, Client player, Client target, string lobbyname ) {
			AddLogEntry ( "vip", command, lobbyname, player.SocialClubName, target != null ? Account.PlayerUIDs[target.SocialClubName].ToString () : "0" );
		}
	}

}
