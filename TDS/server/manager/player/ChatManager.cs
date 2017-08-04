using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	class Chat {
		public static void ChatOnStart ( API api ) {
			api.onChatMessage += OnChatMessage;
		}

		private static void OnChatMessage ( Client player, string message, CancelEventArgs e ) {
			e.Cancel = true;
			Class.Character character = player.GetChar ();
			Log.Chat ( message, player, character.lobby.name );
			string teamfontcolor = character.lobby.teamColorStrings[character.team] ?? "w";
			string changedmessage = "~" + teamfontcolor + "~" + player.socialClubName + "~s~: " + message;
			character.lobby.SendAllPlayerChatMessage ( changedmessage );
		}
	}
}