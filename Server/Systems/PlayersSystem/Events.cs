using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Database.Entity.Player;
using static TDS.Server.Data.Interfaces.PlayersSystem.IPlayerEvents;

namespace TDS.Server.PlayersSystem
{
    public class Events : IPlayerEvents
    {
        public event EntityChangedDelegate? EntityChanged;

        public event EmptyDelegate? Removed;

        public event WeaponSwitchDelegate? WeaponSwitch;

        public event PlayerLobbyDelegate? LobbyJoined;

        public event PlayerLobbyDelegate? LobbyLeft;

        public event EmptyDelegate? SettingsChanged;

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public void TriggerEntityChanged(Players? entity)
            => EntityChanged?.Invoke(entity);

        public void TriggerLobbyJoined(IBaseLobby lobby)
            => LobbyJoined?.Invoke(_player, lobby);

        public void TriggerLobbyLeft(IBaseLobby lobby)
            => LobbyLeft?.Invoke(_player, lobby);

        //Todo: Implement TriggerRemoved for player
        public void TriggerRemoved()
            => Removed?.Invoke();

        public void TriggerWeaponSwitch(WeaponHash previousWeapon, WeaponHash newWeapon)
            => WeaponSwitch?.Invoke(previousWeapon, newWeapon);

        public void TriggerSettingsChanged()
            => SettingsChanged?.Invoke();
    }
}
