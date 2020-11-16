using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.LobbySystem.Weapons
{
    public interface IRoundFightLobbyWeapons
    {
        void GivePlayerWeapons(ITDSPlayer player);

        void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon);
    }
}
