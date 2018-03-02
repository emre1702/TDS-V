namespace TDS.server.manager.lobby {

    using GTANetworkAPI;
    using map;
    using TDS.server.instance.player;

    static class Arena {
        public static instance.lobby.Arena TheLobby;

        public static void Create ( ) {
            TheLobby = new instance.lobby.Arena ( "arena", 1 ) {
                DeleteWhenEmpty = false,
                IsOfficial = true,
            };
            TheLobby.SetMapList ( Map.allMaps, Map.allMapsSync );

            TheLobby.AddTeam ( "Good", (PedHash) ( 2047212121 ), "g" );
            TheLobby.AddTeam ( "Bad", (PedHash) ( 275618457 ), "r" );

            // Handguns //
            TheLobby.AddWeapon ( WeaponHash.Pistol, 1000 ); // Pistol
            TheLobby.AddWeapon ( WeaponHash.CombatPistol, 1000 ); // CombatPistol
            TheLobby.AddWeapon ( WeaponHash.Pistol50, 1000 ); // Pistol50
            TheLobby.AddWeapon ( WeaponHash.SNSPistol, 1000 ); // SNSPistol
            TheLobby.AddWeapon ( WeaponHash.HeavyPistol, 1000 ); // HeavyPistol
            TheLobby.AddWeapon ( WeaponHash.VintagePistol, 1000 ); // VintagePistol
            TheLobby.AddWeapon ( WeaponHash.MarksmanPistol, 1000 ); // MarksmanPistol
            TheLobby.AddWeapon ( WeaponHash.Revolver, 1000 ); // Revolver
            TheLobby.AddWeapon ( WeaponHash.APPistol, 1000 ); // APPistol

            // Machine Guns //
            TheLobby.AddWeapon ( WeaponHash.MicroSMG, 1000 ); // MicroSMG
            TheLobby.AddWeapon ( WeaponHash.MachinePistol, 1000 ); // MachinePistol
            TheLobby.AddWeapon ( WeaponHash.SMG, 1000 ); // SMG
            TheLobby.AddWeapon ( WeaponHash.AssaultSMG, 1000 ); // AssaultSMG
            TheLobby.AddWeapon ( WeaponHash.CombatPDW, 1000 ); // CombatPDW
            TheLobby.AddWeapon ( WeaponHash.MG, 1000 ); // MG
            TheLobby.AddWeapon ( WeaponHash.CombatMG, 1000 ); // CombatMG
            TheLobby.AddWeapon ( WeaponHash.Gusenberg, 1000 ); // Gusenberg
            TheLobby.AddWeapon ( WeaponHash.MiniSMG, 1000 ); // MiniSMG

            // Assault Rifles //
            TheLobby.AddWeapon ( WeaponHash.AssaultRifle, 1000 ); // AssaultRifle
            TheLobby.AddWeapon ( WeaponHash.CarbineRifle, 1000 ); // CarbineRifle
            TheLobby.AddWeapon ( WeaponHash.AdvancedRifle, 1000 ); // AdvancedRifle
            TheLobby.AddWeapon ( WeaponHash.SpecialCarbine, 1000 ); // SpecialCarbine
            TheLobby.AddWeapon ( WeaponHash.BullpupRifle, 1000 ); // BullpupRifle
            TheLobby.AddWeapon ( WeaponHash.CompactRifle, 1000 ); // CompactRifle

            /*// Gunrunning //
            TheLobby.AddWeapon ( WeaponHash) API.GetHashKey ( "WEAPON_PISTOL_MK2" ), 1000 );
            TheLobby.AddWeapon ( WeaponHash) API.GetHashKey ( "WEAPON_SMG_MK2" ), 1000 );
            TheLobby.AddWeapon ( WeaponHash) API.GetHashKey ( "WEAPON_ASSAULTRIFLE_MK2" ), 1000 );
            TheLobby.AddWeapon ( WeaponHash) API.GetHashKey ( "WEAPON_CARBINERIFLE_MK2" ), 1000 );
            TheLobby.AddWeapon ( WeaponHash) API.GetHashKey ( "WEAPON_COMBATMG_MK2" ), 1000 );*/

            TheLobby.StartRoundGame ();
        }

        public static void Join ( Character character, bool spectator ) {
            TheLobby.AddPlayer ( character, spectator );
        }

    }

}
