﻿using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace Manager {
	class MainMenu : Script {
		public static Class.Lobby lobby;

		public MainMenu ( ) {
			lobby = new Class.Lobby ( "mainmenu", 0, false, false );
			lobby.deleteWhenEmpty = false;
		}

		public static void Join ( Client player ) {
			lobby.AddPlayer ( player, true );
			player.triggerEvent ( "onClientJoinMainMenu" );
		}
	}
}