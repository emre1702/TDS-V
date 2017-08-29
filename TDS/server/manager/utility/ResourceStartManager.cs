using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;

namespace Manager {
	class ResourceStart : Script {
		public ResourceStart ( ) {
			StartMethods ( API );
		}

		private static async void StartMethods ( API api ) {
			api.setGamemodeName ( "TDS" );
			Database.DatabaseOnStart ( api );
			Class.Timer.TimerOnStart ( api );
			Class.Damagesys.DamagesysOnStart ( api );
			Class.Lobby.LobbyOnStart ( api );
			Manager.Account.AccountOnStart ( api );
			Manager.MinuteTimer.MinuteTimerOnStart ();
			Manager.Scoreboard.ScoreboardOnStart ( api );
			Manager.Chat.ChatOnStart ( api );
			MainMenu.Create ();

			await Task.Run ( () => Manager.Map.MapOnStart () );
			Arena.Create ();
			GangLobby.Create ();
		}
	}
}