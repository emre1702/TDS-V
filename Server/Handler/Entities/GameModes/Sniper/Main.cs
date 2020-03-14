using System.Collections.Generic;
using System.Linq;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Database.Entity;

namespace TDS_Server.Handler.Entities.GameModes.Sniper
{
    partial class Sniper : GameMode
    {
        public Sniper(Arena lobby, MapDto map) : base(lobby, map) { }

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Where(w => w.Type == TDS_Shared.Data.Enums.EWeaponType.SniperRifle)
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}
