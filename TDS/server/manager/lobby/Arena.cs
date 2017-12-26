namespace TDS.server.manager.lobby {

    using System.Collections.Generic;
    using GTANetworkAPI;
    using instance.lobby;
    using map;
    using TDS.server.enums;
    using TDS.server.instance.utility;

    class Arena : Script {
        public static instance.lobby.Arena TheLobby;

        public Arena ( ) {
            TheLobby = new instance.lobby.Arena ( "arena", 1 ) {
                DeleteWhenEmpty = false,
                IsOfficial = true,
            };
            List<string> mapsforarena = new List<string> ();
            mapsforarena.AddRange ( Map.NormalMapNames );
            mapsforarena.AddRange ( Map.BombMapNames );
            TheLobby.AddMapList ( mapsforarena );
            Dictionary<Language, List<string>> mapdescriptionsforarena = new Dictionary<Language, List<string>> {
                { Language.ENGLISH, new List<string> () },
                { Language.GERMAN, new List<string> () }
            };
            mapdescriptionsforarena[Language.GERMAN].AddRange ( Map.NormalMapDescriptions[Language.GERMAN] );
            mapdescriptionsforarena[Language.ENGLISH].AddRange ( Map.NormalMapDescriptions[Language.ENGLISH] );
            mapdescriptionsforarena[Language.GERMAN].AddRange ( Map.BombMapDescriptions[Language.GERMAN] );
            mapdescriptionsforarena[Language.ENGLISH].AddRange ( Map.BombMapDescriptions[Language.ENGLISH] );
            TheLobby.AddMapDescriptions ( mapdescriptionsforarena );
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

            TheLobby.StartRoundGame ().Wait();
        }

        public static void Join ( Client player ) {
            TheLobby.AddPlayer ( player );
        }

    }

}
