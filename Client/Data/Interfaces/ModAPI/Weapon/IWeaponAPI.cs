using TDS_Shared.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Weapon
{
    public interface IWeaponAPI
    {
        #region Public Methods

        uint GetWeapontypeGroup(WeaponHash weaponHash);

        #endregion Public Methods
    }
}
