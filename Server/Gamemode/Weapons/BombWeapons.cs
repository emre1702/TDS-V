using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.GamemodesSystem.Gamemodes;

namespace TDS_Server.GamemodesSystem.Weapons
{
    public class BombWeapons : BaseGamemodeWeapons
    {
        private readonly BombGamemode _gamemode;

        public BombWeapons(BombGamemode gamemode)
            => _gamemode = gamemode;

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);

            events.PlayerWeaponSwitch += Events_PlayerWeaponSwitch;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);

            events.PlayerWeaponSwitch -= Events_PlayerWeaponSwitch;
        }

        private void Events_PlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            if (_gamemode.Players.BombAtPlayer == player)
                _gamemode.Specials.ToggleBombAtHand(player, oldWeapon, newWeapon);
        }
    }
}
