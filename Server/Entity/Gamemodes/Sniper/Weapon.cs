using System.Collections.Generic;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.Gamemodes.Sniper
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
