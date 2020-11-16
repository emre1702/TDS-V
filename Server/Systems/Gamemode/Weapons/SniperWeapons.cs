using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Database.Entity;
using TDS.Shared.Data.Enums;

namespace TDS.Server.GamemodesSystem.Weapons
{
    public class SniperWeapons : BaseGamemodeWeapons
    {
        private static HashSet<WeaponHash> _allowedWeaponHashes = new HashSet<WeaponHash>();

        internal static void LoadAllowedWeapons(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
               .Where(w => w.Type == WeaponType.SniperRifle)
               .Select(w => w.Hash)
               .ToHashSet();
        }

        public override bool IsWeaponAllowed(WeaponHash weaponHash)
            => _allowedWeaponHashes.Contains(weaponHash);
    }
}
