using GTANetworkAPI;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Weapons
{
    public interface IBaseGamemodeWeapons
    {
        bool HandlesGivingWeapons { get; }

        bool IsWeaponAllowed(WeaponHash weaponHash);
    }
}
