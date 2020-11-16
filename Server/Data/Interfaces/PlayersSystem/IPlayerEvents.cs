using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
#nullable enable

    public interface IPlayerEvents
    {
        delegate void EntityChangedDelegate(Players? entity);

        delegate void EmptyDelegate();

        delegate void PlayerLobbyDelegate(ITDSPlayer player, IBaseLobby lobby);

        delegate void WeaponSwitchDelegate(WeaponHash previousWeapon, WeaponHash newWeapon);

        event EntityChangedDelegate? EntityChanged;

        event PlayerLobbyDelegate? LobbyJoined;

        event PlayerLobbyDelegate? LobbyLeft;

        event EmptyDelegate? Removed;

        event EmptyDelegate? SettingsChanged;

        event WeaponSwitchDelegate? WeaponSwitch;

        void Init(ITDSPlayer player);

        void TriggerEntityChanged(Players? entity);

        void TriggerLobbyJoined(IBaseLobby lobby);

        void TriggerLobbyLeft(IBaseLobby lobby);

        void TriggerRemoved();

        void TriggerSettingsChanged();

        void TriggerWeaponSwitch(WeaponHash previousWeapon, WeaponHash newWeapon);
    }
}
