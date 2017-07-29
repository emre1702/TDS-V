using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;

namespace Manager {
	static class Arena {
		public static Class.Lobby lobby;

		public static void Create ( ) {
			lobby = new Class.Lobby ( "arena", 1 );
			lobby.AddMapList ( Manager.Map.mapNames, false );
			lobby.AddTeam ( "Gut", (PedHash) ( 2047212121 ) );
			lobby.AddTeam ( "Böse", (PedHash) ( 275618457 ) );
			lobby.deleteWhenEmpty = false;
			lobby.AddWeapon ( (WeaponHash) ( 453432689 ), 1000 );
			lobby.AddWeapon ( (WeaponHash) ( 736523883 ), 1000 );
			lobby.AddWeapon ( (WeaponHash) ( -2084633992 ), 1000 );
			lobby.AddWeapon ( (WeaponHash) ( -1466123874 ), 1000 );
			lobby.Start ();
		}

		public static void Join ( Client player ) {
			lobby.AddPlayer ( player );
		}
		
	}
}
