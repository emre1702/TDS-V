namespace TDS.server.manager.utility {

	using extend;
	using GTANetworkAPI;
	using instance.player;
	using logs;

	class Chat : Script {
		internal static Chat Instance;

		public Chat () {
			Event.OnChatMessage += OnChatMessage;
			Instance = this;
		}

		private static void OnChatMessageFunc ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "chat " + character.Lobby.Name );
			string teamfontcolor = character.Lobby.TeamColorStrings[character.Team] ?? "w";
			string changedmessage = "~" + teamfontcolor + "~" + player.SocialClubName + "~s~: " + message;
			character.Lobby.SendAllPlayerChatMessage ( changedmessage );
		}

		private static void OnChatMessage ( Client player, string message, CancelEventArgs e ) {
			e.Cancel = true;
			OnChatMessageFunc ( player, message );
		}

		public void SendGlobalMessage ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "global" );
			string teamfontcolor = character.Lobby.TeamColorStrings[character.Team];
			string changedmessage = "[GLOBAL] ~" + teamfontcolor + "~" + player.SocialClubName + "~s~: " + message;
			this.API.SendChatMessageToAll ( changedmessage );
		}

		public void SendAdminMessage ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "osay" );
			string changedmessage = Admin.LevelFontColor[character.AdminLvl] + "[" + Admin.NameByLevel[character.AdminLvl] + "] ~w~" + player.SocialClubName + ": ~s~" + message;
			this.API.SendChatMessageToAll ( changedmessage );
		}

		public void SendAdminChat ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "achat" );
			string changedmessage = "~w~[ADMINCHAT] " + Admin.LevelFontColor[character.AdminLvl] + player.SocialClubName + ": ~s~" + message;
			Admin.SendChatMessageToAdmins ( changedmessage );
		}

		public void SendTeamChat ( Client player, string message ) {
			Character character = player.GetChar ();
			Log.Chat ( message, player, "team" );
			string teamfontcolor = character.Lobby.TeamColorStrings[character.Team];
			string changedmessage = "[TEAM] ~" + teamfontcolor + "~" + player.SocialClubName + "~s~: " + message;
			character.Lobby.SendAllPlayerChatMessage ( changedmessage, (int) character.Team );
		}

	}

}
