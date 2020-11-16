using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.GamemodesSystem.Weapons
{
    public interface IArmsRaceGamemodeWeapons
    {
        bool HandlesGivingWeapons { get; }

        WeaponHash GetCurrentWeapon(ITDSPlayer player);

        bool TryGetNextWeapon(ITDSPlayer player, out WeaponHash? weaponHash);

        bool IsWeaponAllowed(WeaponHash weaponHash);
    }
}
