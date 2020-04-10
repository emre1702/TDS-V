using TDS_Shared.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Weapon
{
    public interface IWeaponAPI
    {
        uint GetWeapontypeGroup(WeaponHash weaponHash);
    }
}
