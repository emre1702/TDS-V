using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Entity.Gamemodes;
using TDS_Server.Entity.Gamemodes.ArmsRace;
using TDS_Server.Entity.Gamemodes.Bomb;
using TDS_Server.Entity.Gamemodes.Gangwar;
using TDS_Server.Entity.Gamemodes.Sniper;
using TDS_Server.Entity.Gamemodes.TeamDeathmatch;

namespace TDS_Server.Entity
{
    public class EntitiesStaticConnector : IEntitiesStaticConnector
    {
        public HashSet<LobbyWeapons> GetAllowedWeapons(MapType type)
            => type switch
            {
                MapType.Bomb => Bomb.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
                MapType.Sniper => Sniper.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
                MapType.Gangwar => Gangwar.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
                MapType.ArmsRace => ArmsRace.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
                _ => Deathmatch.GetAllowedWeapons().Select(w => new LobbyWeapons { Hash = w, Ammo = 9999, Damage = 0 }).ToHashSet(),
            };

        public void InitGamemodes(TDSDbContext dbContext)
        {
            Gamemode.Init(dbContext);
            Sniper.Init(dbContext);
        }
    }
}
