using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	class Chat {
		public static void ChatOnStart ( API api ) {
			api.onChatMessage += OnChatMessage;
		}

		private static void OnChatMessageFunc ( Client player, string message ) {
			Class.Character character = player.GetChar ();
			Log.Chat ( message, player, "chat " + character.lobby.name );
			string teamfontcolor = character.lobby.teamColorStrings[character.team] ?? "w";
			string changedmessage = "~" + teamfontcolor + "~" + player.socialClubName + "~s~: " + message;
			character.lobby.SendAllPlayerChatMessage ( changedmessage );
		}

		private static void OnChatMessage ( Client player, string message, CancelEventArgs e ) {
			e.Cancel = true;
			OnChatMessageFunc ( player, message );
		}

		public static void SendGlobalMessage ( Client player, string message ) {
			Class.Character character = player.GetChar ();
			Log.Chat ( message, player, "global" );
			string teamfontcolor = character.lobby.teamColorStrings[character.team];
			string changedmessage = "[GLOBAL] ~" + teamfontcolor + "~" + player.socialClubName + "~s~: " + message;
			API.shared.sendChatMessageToAll ( changedmessage );
		}

		public static void SendAdminMessage ( Client player, string message ) {
			Class.Character character = player.GetChar ();
			Log.Chat ( message, player, "osay" );
			string changedmessage = Admin.levelFontColor[character.adminLvl] + "[" + Admin.nameByLevel[character.adminLvl] + "] ~w~" + player.socialClubName + ": ~s~" + message;
			API.shared.sendChatMessageToAll ( changedmessage );
		}

		public static void SendAdminChat ( Client player, string message ) {
			Class.Character character = player.GetChar ();
			Log.Chat ( message, player, "achat" );
			string changedmessage = "~w~[ADMINCHAT] " + Admin.levelFontColor[character.adminLvl] + player.socialClubName + ": ~s~" + message;
			Admin.SendChatMessageToAdmins ( changedmessage );
		}

		public static void SendTeamChat ( Client player, string message ) {
			Class.Character character = player.GetChar ();
			Log.Chat ( message, player, "team" );
			string teamfontcolor = character.lobby.teamColorStrings[character.team];
			string changedmessage = "[TEAM] ~" + teamfontcolor + "~" + player.socialClubName + "~s~: " + message;
			character.lobby.SendAllPlayerChatMessage ( message, character.team );
		}

	}
}