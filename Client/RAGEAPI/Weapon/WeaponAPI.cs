using TDS_Client.Data.Interfaces.RAGE.Game.Weapon;
using TDS_Shared.Data.Enums;

namespace TDS_Client.RAGEAPI.Weapon
{
    internal class WeaponAPI : IWeaponAPI
    {
        #region Public Methods

        public uint GetWeapontypeGroup(WeaponHash weaponHash)
        {
            return RAGE.Game.Weapon.GetWeapontypeGroup((uint)weaponHash);
        }

        #endregion Public Methods
    }
}