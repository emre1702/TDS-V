using TDS_Client.Data.Interfaces.ModAPI.Weapon;
using TDS_Shared.Data.Enums;

namespace TDS_Client.RAGEAPI.Weapon
{
    class WeaponAPI : IWeaponAPI
    {
        public uint GetWeapontypeGroup(WeaponHash weaponHash)
        {
            return RAGE.Game.Weapon.GetWeapontypeGroup((uint)weaponHash);
        }
    }
}
