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

			// Handguns //
			lobby.AddWeapon ( (WeaponHash) ( 453432689 ), 1000 ); // Pistol
			lobby.AddWeapon ( (WeaponHash) ( 1593441988 ), 1000 ); // CombatPistol
			lobby.AddWeapon ( (WeaponHash) ( -1716589765 ), 1000 ); // Pistol50
			lobby.AddWeapon ( (WeaponHash) ( -1076751822 ), 1000 ); // SNSPistol
			lobby.AddWeapon ( (WeaponHash) ( -771403250 ), 1000 ); // HeavyPistol
			lobby.AddWeapon ( (WeaponHash) ( 137902532 ), 1000 ); // VintagePistol
			lobby.AddWeapon ( (WeaponHash) ( -598887786 ), 1000 ); // MarksmanPistol
			lobby.AddWeapon ( (WeaponHash) ( -1045183535 ), 1000 ); // Revolver
			lobby.AddWeapon ( (WeaponHash) ( -584646201 ), 1000 ); // APPistol

			// Machine Guns //
			lobby.AddWeapon ( (WeaponHash) ( 324215364 ), 1000 ); // MicroSMG
			lobby.AddWeapon ( (WeaponHash) ( -619010992 ), 1000 ); // MachinePistol
			lobby.AddWeapon ( (WeaponHash) ( 736523883 ), 1000 ); // SMG
			lobby.AddWeapon ( (WeaponHash) ( -270015777 ), 1000 ); // AssaultSMG
			lobby.AddWeapon ( (WeaponHash) ( 171789620 ), 1000 ); // CombatPDW
			lobby.AddWeapon ( (WeaponHash) ( -1660422300 ), 1000 ); // MG
			lobby.AddWeapon ( (WeaponHash) ( 2144741730 ), 1000 ); // CombatMG
			lobby.AddWeapon ( (WeaponHash) ( 1627465347 ), 1000 ); // Gusenberg
			lobby.AddWeapon ( (WeaponHash) ( -1121678507 ), 1000 ); // MiniSMG

			// Assault Rifles //
			lobby.AddWeapon ( (WeaponHash) ( -1074790547 ), 1000 ); // AssaultRifle
			lobby.AddWeapon ( (WeaponHash) ( -2084633992 ), 1000 ); // CarbineRifle
			lobby.AddWeapon ( (WeaponHash) ( -1357824103 ), 1000 ); // AdvancedRifle
			lobby.AddWeapon ( (WeaponHash) ( -1063057011 ), 1000 ); // SpecialCarbine
			lobby.AddWeapon ( (WeaponHash) ( 2132975508 ), 1000 ); // BullpupRifle
			lobby.AddWeapon ( (WeaponHash) ( 1649403952 ), 1000 ); // CompactRifle

			lobby.Start ();
		}

		public static void Join ( Client player ) {
			lobby.AddPlayer ( player );
		}
		
	}
}
