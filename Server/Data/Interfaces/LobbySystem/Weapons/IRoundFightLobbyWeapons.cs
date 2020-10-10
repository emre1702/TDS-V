using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Weapons
{
    public interface IRoundFightLobbyWeapons
    {
        void GivePlayerWeapons(ITDSPlayer player);

        void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon);
    }
}
