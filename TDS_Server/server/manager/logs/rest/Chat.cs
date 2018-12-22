namespace TDS.server.manager.logs {

	using GTANetworkAPI;
	using TDS.server.enums;

	partial class Log {

		private static string GetChatType ( ref string chat ) {
			if ( chat.EndsWith ( "$normal$" ) )
				return "normal";
			else if ( chat.EndsWith ( "$dirty$" ) )
				return "dirty";
			else
				return "0";
		}

		public static void Chat ( ref string chatstr, Client player, string lobbyname, uint targetUID = 0 ) {
			string chatinfoextended = targetUID != 0 ? targetUID.ToString() : GetChatType ( ref chatstr );
			AddLogEntry ( LogType.CHAT, chatstr, lobbyname, player.SocialClubName, targetUID: chatinfoextended );
		}
	}

}
