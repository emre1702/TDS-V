using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.Database.SeedData.Lobby
{
    public static class LobbyWeaponsSeeds
    {
        public static ModelBuilder HasLobbyWeapons(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LobbyWeapons>().HasData(
                new LobbyWeapons { Hash = WeaponHash.Pistol50, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Revolver, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Smg, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Combatpdw, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Musket, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Sawnoffshotgun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Assaultrifle, Lobby = -1, Ammo = 9999 }
            );
            return modelBuilder;
        }
    }
}
