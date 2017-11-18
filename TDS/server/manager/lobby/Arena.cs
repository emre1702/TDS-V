namespace TDS.server.manager.lobby {

	using System.Collections.Generic;
	using GTANetworkAPI;
	using instance.lobby;
	using map;

	class Arena : Script {
		public static Lobby TheLobby;

		public Arena () {
			TheLobby = new Lobby ( "arena", 1 ) {
				DeleteWhenEmpty = false,
				IsOfficial = true
			};
			List<string> mapsforarena = new List<string> ();
			mapsforarena.AddRange ( Map.NormalMapNames );
			mapsforarena.AddRange ( Map.BombMapNames );
			TheLobby.AddMapList ( mapsforarena );
			Dictionary<string, List<string>> mapdescriptionsforarena = new Dictionary<string, List<string>> {
				{
					"german", new List<string> ()
				}, {
					"english", new List<string> ()
				}
			};
			mapdescriptionsforarena["german"].AddRange ( Map.NormalMapDescriptions["german"] );
			mapdescriptionsforarena["english"].AddRange ( Map.NormalMapDescriptions["english"] );
			mapdescriptionsforarena["german"].AddRange ( Map.BombMapDescriptions["german"] );
			mapdescriptionsforarena["english"].AddRange ( Map.BombMapDescriptions["english"] );
			TheLobby.AddMapDescriptions ( mapdescriptionsforarena );
			TheLobby.AddTeam ( "Good", (PedHash) ( 2047212121 ), "g" );
			TheLobby.AddTeam ( "Bad", (PedHash) ( 275618457 ), "r" );

			unchecked {
				// Handguns //
				TheLobby.AddWeapon ( (WeaponHash) ( 453432689 ), 1000 ); // Pistol
				TheLobby.AddWeapon ( (WeaponHash) ( 1593441988 ), 1000 ); // CombatPistol
				TheLobby.AddWeapon ( (WeaponHash) ( -1716589765 ), 1000 ); // Pistol50
				TheLobby.AddWeapon ( (WeaponHash) ( -1076751822 ), 1000 ); // SNSPistol
				TheLobby.AddWeapon ( (WeaponHash) ( -771403250 ), 1000 ); // HeavyPistol
				TheLobby.AddWeapon ( (WeaponHash) ( 137902532 ), 1000 ); // VintagePistol
				TheLobby.AddWeapon ( (WeaponHash) ( -598887786 ), 1000 ); // MarksmanPistol
				TheLobby.AddWeapon ( (WeaponHash) ( -1045183535 ), 1000 ); // Revolver
				TheLobby.AddWeapon ( (WeaponHash) ( -584646201 ), 1000 ); // APPistol

				// Machine Guns //
				TheLobby.AddWeapon ( (WeaponHash) ( 324215364 ), 1000 ); // MicroSMG
				TheLobby.AddWeapon ( (WeaponHash) ( -619010992 ), 1000 ); // MachinePistol
				TheLobby.AddWeapon ( (WeaponHash) ( 736523883 ), 1000 ); // SMG
				TheLobby.AddWeapon ( (WeaponHash) ( -270015777 ), 1000 ); // AssaultSMG
				TheLobby.AddWeapon ( (WeaponHash) ( 171789620 ), 1000 ); // CombatPDW
				TheLobby.AddWeapon ( (WeaponHash) ( -1660422300 ), 1000 ); // MG
				TheLobby.AddWeapon ( (WeaponHash) ( 2144741730 ), 1000 ); // CombatMG
				TheLobby.AddWeapon ( (WeaponHash) ( 1627465347 ), 1000 ); // Gusenberg
				TheLobby.AddWeapon ( (WeaponHash) ( -1121678507 ), 1000 ); // MiniSMG

				// Assault Rifles //
				TheLobby.AddWeapon ( (WeaponHash) ( -1074790547 ), 1000 ); // AssaultRifle
				TheLobby.AddWeapon ( (WeaponHash) ( -2084633992 ), 1000 ); // CarbineRifle
				TheLobby.AddWeapon ( (WeaponHash) ( -1357824103 ), 1000 ); // AdvancedRifle
				TheLobby.AddWeapon ( (WeaponHash) ( -1063057011 ), 1000 ); // SpecialCarbine
				TheLobby.AddWeapon ( (WeaponHash) ( 2132975508 ), 1000 ); // BullpupRifle
				TheLobby.AddWeapon ( (WeaponHash) ( 1649403952 ), 1000 ); // CompactRifle

				// Gunrunning //
				TheLobby.AddWeapon ( (WeaponHash) this.API.GetHashKey ( "WEAPON_PISTOL_MK2" ), 1000 );
				TheLobby.AddWeapon ( (WeaponHash) this.API.GetHashKey ( "WEAPON_SMG_MK2" ), 1000 );
				TheLobby.AddWeapon ( (WeaponHash) this.API.GetHashKey ( "WEAPON_ASSAULTRIFLE_MK2" ), 1000 );
				TheLobby.AddWeapon ( (WeaponHash) this.API.GetHashKey ( "WEAPON_CARBINERIFLE_MK2" ), 1000 );
				TheLobby.AddWeapon ( (WeaponHash) this.API.GetHashKey ( "WEAPON_COMBATMG_MK2" ), 1000 );
			}

			TheLobby.Start ();
		}

		public static void Join ( Client player ) {
			TheLobby.AddPlayer ( player );
		}

	}

}
