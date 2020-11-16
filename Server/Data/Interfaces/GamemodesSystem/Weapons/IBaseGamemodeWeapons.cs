using GTANetworkAPI;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.Weapons
{
    public interface IBaseGamemodeWeapons
    {
        bool HandlesGivingWeapons { get; }

        bool IsWeaponAllowed(WeaponHash weaponHash);
    }
}
