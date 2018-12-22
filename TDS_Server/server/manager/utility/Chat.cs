namespace TDS.server.manager.utility {

	using extend;
	using GTANetworkAPI;
	using instance.player;
	using logs;

	class Chat : Script {

        public Chat ( ) { }

		private static void OnChatMessageFunc ( Character character, string message ) {
			Log.Chat ( ref message, character.Player, "chat " + character.Lobby.Name );
			string teamfontcolor = character.Lobby.TeamColorStrings[character.Team] ?? "w";
			string changedmessage = "#" + teamfontcolor + "#" + character.Player.SocialClubName + "#s#: " + message;
			character.Lobby.SendAllPlayerChatMessage ( changedmessage );
		}

        [DisableDefaultChat]
        [ServerEvent(Event.ChatMessage)]
        public static void OnChatMessage ( Client player, string message ) {
            Character character = player.GetChar ();
			if ( character.LoggedIn )
				if ( character.MuteTime == 0 )
					OnChatMessageFunc( character, message );
				else if ( character.MuteTime == 1 )
					player.SendLangNotification( "you_are_still_permamuted" );
				else
					player.SendLangNotification( "you_are_still_muted", character.MuteTime.ToString() );


		}

		public static void SendGlobalMessage ( Character character, string message ) {
			Log.Chat ( ref message, character.Player, "global" );
			string teamfontcolor = character.Lobby.TeamColorStrings[character.Team];
			string changedmessage = "[GLOBAL] #" + teamfontcolor + "#" + character.Player.SocialClubName + "#s#: " + message;
			NAPI.Chat.SendChatMessageToAll ( changedmessage );
		}

		public static void SendAdminMessage ( Character character, string message ) {
			Log.Chat ( ref message, character.Player, "osay" );
			string changedmessage = Admin.LevelFontColor[character.AdminLvl] + "[" + Admin.NameByLevel[character.AdminLvl] + "] #w#" + character.Player.SocialClubName + ": #s#" + message;
			NAPI.Chat.SendChatMessageToAll ( changedmessage );
		}

		public static void SendAdminChat ( Character character, string message ) {
			Log.Chat ( ref message, character.Player, "achat" );
			string changedmessage = "[ADMINCHAT] " + Admin.LevelFontColor[character.AdminLvl] + character.Player.SocialClubName + ": #s#" + message;
			Admin.SendChatMessageToAdmins ( changedmessage );
		}

		public static void SendTeamChat ( Character character, string message ) {
			Log.Chat ( ref message, character.Player, "team" );
			string teamfontcolor = character.Lobby.TeamColorStrings[character.Team];
			string changedmessage = "[TEAM] #" + teamfontcolor + "#" + character.Player.SocialClubName + "#s#: " + message;
			character.Lobby.SendAllPlayerChatMessage ( changedmessage, character.Team );
		}

        public static void SendPrivateMessage ( Character character, Character targetcharacter, string message ) {
            Log.Chat ( ref message, character.Player, "private", targetcharacter.UID );
            string changedmessage = "[PM] #o#" + character.Player.SocialClubName + "#s#: " + message;
            NAPI.Chat.SendChatMessageToPlayer ( targetcharacter.Player, changedmessage );
        }

	}

}
