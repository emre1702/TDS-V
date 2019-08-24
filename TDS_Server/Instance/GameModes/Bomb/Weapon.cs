using System;
using System.Collections.Generic;
using System.Text;
using TDS_Common.Enum;

namespace TDS_Server.Instance.GameModes
{
    partial class Bomb
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
