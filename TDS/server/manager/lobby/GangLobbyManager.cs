using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace Manager {
	class GangLobby : Script {

		public static Class.Lobby lobby;

		public GangLobby ( ) {
			lobby = new Class.Lobby ( "gang", 2, false );
			lobby.deleteWhenEmpty = false;
			lobby.isOfficial = true;
			lobby.AddSpawnPoint ( new Vector3 ( 8.699318, -2.050943, 70.29607 ), new Vector3 ( 0, 0, 65.79314 ) );
			lobby.Start ();
		}

		public static void Join ( Client player ) {
			lobby.AddPlayer ( player, true );
		}
	}
}
