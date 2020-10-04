using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers
{
#nullable enable

    public interface IFightLobbyEventsHandler : IBaseLobbyEventsHandler
    {
        public delegate void PlayerDiedDelegate(ITDSPlayer player, ITDSPlayer killer, uint weapon, int hadLifes);

        public delegate void PlayerWeaponSwitchDelegate(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon);

        event PlayerDiedDelegate? PlayerDied;

        event PlayerWeaponSwitchDelegate? PlayerWeaponSwitch;

        void TriggerPlayerDied(ITDSPlayer player, ITDSPlayer killer, uint weapon, int hadLifes);

        void TriggerPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon);
    }
}
