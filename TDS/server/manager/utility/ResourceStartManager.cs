using System;
using GrandTheftMultiplayer.Server.API;

namespace Manager {
	class ResourceStart : Script {
		public ResourceStart ( ) {
			StartMethods ( API );
		}

		private static async void StartMethods ( API api ) {
			try {
				api.setGamemodeName ( "TDS" );
				api.setMapName ( "Los Santos" );
				Database.DatabaseOnStart ( api );
				Class.Timer.TimerOnStart ( api );
				Class.Damagesys.DamagesysOnStart ( api );
				Class.Lobby.LobbyOnStart ( api );
				Manager.Account.AccountOnStart ( api );
				Manager.MinuteTimer.MinuteTimerOnStart ();
				Manager.Scoreboard.ScoreboardOnStart ( api );
				Manager.Chat.ChatOnStart ( api );
				MainMenu.Create ();

				await Manager.Map.MapOnStart ().ConfigureAwait ( false );
				Arena.Create ();
				GangLobby.Create ();
			} catch ( Exception ex ) {
				API.shared.consoleOutput ( "Error in StartMethods:" + ex.Message );
			}
		}
	}
}