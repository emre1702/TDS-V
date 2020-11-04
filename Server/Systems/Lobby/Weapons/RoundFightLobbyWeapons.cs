using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.LobbySystem.Weapons
{
    public class RoundFightLobbyWeapons : FightLobbyWeapons, IRoundFightLobbyWeapons
    {
        private List<LobbyWeapons> _allRoundWeapons = new List<LobbyWeapons>();

        protected new IRoundFightLobby Lobby => (IRoundFightLobby)base.Lobby;
        protected new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;

        public RoundFightLobbyWeapons(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events) : base(lobby, events)
        {
            events.WeaponsLoading += Events_WeaponsLoading;
            events.PlayerWeaponSwitch += OnPlayerWeaponSwitch;
            events.RemoveAfter += RemoveEvents;
        }

        private void RemoveEvents(IBaseLobby _)
        {
            Events.WeaponsLoading -= Events_WeaponsLoading;
            Events.PlayerWeaponSwitch -= OnPlayerWeaponSwitch;
            Events.RemoveAfter -= RemoveEvents;
        }

        public override void GivePlayerWeapons(ITDSPlayer player)
        {
            if (_allRoundWeapons is null)
                return;
            if (Lobby.Rounds.CurrentGamemode?.Weapons.HandlesGivingWeapons == true)
                return;
            base.GivePlayerWeapons(player);
        }

        internal void SetRoundWeapons(List<LobbyWeapons> weapons)
            => _allRoundWeapons = weapons;

        internal override IEnumerable<LobbyWeapons> GetAllWeapons()
            => _allRoundWeapons;

        public virtual void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
        }

        private void Events_WeaponsLoading()
        {
            _allRoundWeapons = Lobby.Entity.LobbyWeapons
                .Where(w => Lobby.Rounds.CurrentGamemode?.Weapons.IsWeaponAllowed(w.Hash) != false)
                .ToList();
        }
    }
}
