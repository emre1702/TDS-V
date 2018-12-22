namespace TDS.server.manager.logs {

	using GTANetworkAPI;
	using player;
	using TDS.server.enums;

	partial class Log {
		public static void VIP ( string command, Client player, Client target, string lobbyname ) {
			AddLogEntry ( LogType.VIP, command, lobbyname, player.SocialClubName, target != null ? Account.PlayerUIDs[target.SocialClubName].ToString () : "0" );
		}
	}

}
