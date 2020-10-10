using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.GamemodesSystem.Weapons
{
    public interface IArmsRaceGamemodeWeapons
    {
        bool HandlesGivingWeapons { get; }

        WeaponHash GetCurrentWeapon(ITDSPlayer player);

        bool GetNextWeapon(ITDSPlayer player, out WeaponHash? weaponHash);

        bool IsWeaponAllowed(WeaponHash weaponHash);
    }
}
