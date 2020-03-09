using GTANetworkAPI;
using System.Collections.Generic;

namespace TDS_Server.Core.Instance.GameModes.Bomb
{
    partial class Bomb
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
