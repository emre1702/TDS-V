using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace TDS_Server.Handler.Entities.GameModes.Normal
{
    partial class Normal
    {
        private static HashSet<WeaponHash> _allowedWeaponHashes = new HashSet<WeaponHash>();

        public static HashSet<WeaponHash> GetAllowedWeapons()
        {
            return _allowedWeaponHashes;
        }

        public override bool IsWeaponAllowed(WeaponHash weaponHash)
        {
            return _allowedWeaponHashes.Contains(weaponHash);
        }
    }
}
