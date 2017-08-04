﻿using GrandTheftMultiplayer.Server.API;

namespace Manager {
	class ResourceStart : Script {
		public ResourceStart ( ) {
			API.shared.setGamemodeName ( "TDS" );
			Database.DatabaseOnStart ( API );
			Class.Timer.TimerOnStart ( API );
			Manager.Map.MapOnStart ();
			Class.Damagesys.DamagesysOnStart ( API );
			Class.Lobby.LobbyOnStart ( API );
			Manager.Account.AccountOnStart ( API );
			Manager.MinuteTimer.MinuteTimerOnStart ();
			Manager.Scoreboard.ScoreboardOnStart ( API );
			Manager.Chat.ChatOnStart ( API );

			MainMenu.Create ();
			Arena.Create ();
		}
	}
}