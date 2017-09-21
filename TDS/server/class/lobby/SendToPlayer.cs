using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;

namespace Class {

	partial class Lobby {

		public void SendAllPlayerEvent ( string eventName, int teamindex = -1, params object[] args ) {
			if ( teamindex == -1 ) {
				this.FuncIterateAllPlayers ( ( player, teamID ) => {
					player.triggerEvent ( eventName, args );
				} );
			} else
				this.FuncIterateAllPlayers ( ( player, teamID ) => {
					player.triggerEvent ( eventName, args );
				}, teamindex );
		}

		public void SendAllPlayerLangNotification ( string langstr, int teamindex = -1, params string[] args ) {
			Dictionary<string, string> texts = Language.GetLangDictionary ( langstr, args );
			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				API.shared.sendNotificationToPlayer ( player, texts[player.GetChar().language] );
			}, teamindex );
		}

		public void SendAllPlayerChatMessage ( string message, int teamindex = -1 ) {
			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				player.sendChatMessage ( message );
			}, teamindex );
		}
	}
}