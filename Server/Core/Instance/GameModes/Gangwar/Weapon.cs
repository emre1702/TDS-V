﻿using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Server.Core.Instance.GameModes.Gangwar
{
    partial class Gangwar
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