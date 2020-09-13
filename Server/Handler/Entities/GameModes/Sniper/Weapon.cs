﻿using GTANetworkAPI;
using System.Collections.Generic;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class Sniper
    {
        #region Private Fields

        private static HashSet<WeaponHash> _allowedWeaponHashes = new HashSet<WeaponHash>();

        #endregion Private Fields

        #region Public Methods

        public static new HashSet<WeaponHash> GetAllowedWeapons()
        {
            return _allowedWeaponHashes;
        }

        public override bool IsWeaponAllowed(WeaponHash weaponHash)
        {
            return _allowedWeaponHashes.Contains(weaponHash);
        }

        #endregion Public Methods
    }
}