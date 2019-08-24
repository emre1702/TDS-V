using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Server.Instance.GameModes
{
    partial class Sniper
    {
        private static HashSet<EWeaponHash> _allowedWeaponHashes = new HashSet<EWeaponHash>();

        public static HashSet<EWeaponHash> GetAllowedWeapons()
        {
            return _allowedWeaponHashes;
        }

        public override bool IsWeaponAllowed(EWeaponHash weaponHash)
        {
            return _allowedWeaponHashes.Contains(weaponHash);
        }
    }
}
