using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Database.Entity.Player;
using static TDS_Server.Data.Interfaces.PlayersSystem.IPlayerEvents;

namespace TDS_Server.PlayersSystem
{
    public class Events : IPlayerEvents
    {
        public event EntityChangedDelegate? EntityChanged;

        public event EmptyDelegate? Removed;

        public event WeaponSwitchDelegate? WeaponSwitch;

        public event PlayerLobbyDelegate? LobbyJoined;

        public event PlayerLobbyDelegate? LobbyLeft;

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
    }
}
