namespace TDS.server.manager.utility {

	using extend;
	using GTANetworkAPI;
	using instance.player;
	using logs;

	class Chat : Script {

        public Chat ( ) { }

		private static void OnChatMessageFunc ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "chat " + character.Lobby.Name );
			string teamfontcolor = character.Lobby.TeamColorStrings[character.Team] ?? "w";
			string changedmessage = "#" + teamfontcolor + "#" + player.SocialClubName + "#s#: " + message;
			character.Lobby.SendAllPlayerChatMessage ( changedmessage );
		}

        [DisableDefaultChat]
        [ServerEvent(Event.ChatMessage)]
        public static void OnChatMessage ( Client player, string message ) {
            if ( player.GetChar().LoggedIn )
			    OnChatMessageFunc ( player, message );
		}

		public static void SendGlobalMessage ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "global" );
			string teamfontcolor = character.Lobby.TeamColorStrings[character.Team];
			string changedmessage = "[GLOBAL] #" + teamfontcolor + "#" + player.SocialClubName + "#s#: " + message;
			NAPI.Chat.SendChatMessageToAll ( changedmessage );
		}

		public static void SendAdminMessage ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "osay" );
			string changedmessage = Admin.LevelFontColor[character.AdminLvl] + "[" + Admin.NameByLevel[character.AdminLvl] + "] #w#" + player.SocialClubName + ": #s#" + message;
			NAPI.Chat.SendChatMessageToAll ( changedmessage );
		}

		public static void SendAdminChat ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "achat" );
			string changedmessage = "[ADMINCHAT] " + Admin.LevelFontColor[character.AdminLvl] + player.SocialClubName + ": #s#" + message;
			Admin.SendChatMessageToAdmins ( changedmessage );
		}

		public static void SendTeamChat ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "team" );
			string teamfontcolor = character.Lobby.TeamColorStrings[character.Team];
			string changedmessage = "[TEAM] #" + teamfontcolor + "#" + player.SocialClubName + "#s#: " + message;
			character.Lobby.SendAllPlayerChatMessage ( changedmessage, (int) character.Team );
		}

        public static void SendPrivateMessage ( Client player, Client target, string message ) {
            Log.Chat ( message, player, "private", target.GetChar().UID );
            string changedmessage = "[PM] #o#" + player.SocialClubName + "#s#: " + message;
            target.SendChatMessage ( changedmessage );
        }

	}

}
