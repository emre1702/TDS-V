using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.SeedData.Lobby
{
    public static class LobbyArmsRaceWeaponsSeeds
    {
        public static ModelBuilder HasLobbyArmsRaceWeapons(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LobbyArmsRaceWeapons>().HasData(
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 0, WeaponHash = WeaponHash.Microsmg },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 1, WeaponHash = WeaponHash.Assaultsmg },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 2, WeaponHash = WeaponHash.Machinepistol },

                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 3, WeaponHash = WeaponHash.Assaultrifle },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 4, WeaponHash = WeaponHash.Carbinerifle },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 5, WeaponHash = WeaponHash.Advancedrifle },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 6, WeaponHash = WeaponHash.Specialcarbine },

                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 7, WeaponHash = WeaponHash.Pumpshotgun },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 8, WeaponHash = WeaponHash.Assaultshotgun },

                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 9, WeaponHash = WeaponHash.Heavysniper },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 10, WeaponHash = WeaponHash.Marksmanrifle },

                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 11, WeaponHash = WeaponHash.Combatmg },

                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 12, WeaponHash = WeaponHash.Combatpistol },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 13, WeaponHash = WeaponHash.Pistol50 },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 14, WeaponHash = WeaponHash.Heavypistol },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 15, WeaponHash = WeaponHash.Revolver },
                new LobbyArmsRaceWeapons { LobbyId = -1, AtKill = 16, WeaponHash = null }
            );
            return modelBuilder;
        }
    }
}
