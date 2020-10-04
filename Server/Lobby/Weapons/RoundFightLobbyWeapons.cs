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

        public RoundFightLobbyWeapons(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events) : base(lobby)
        {
            events.WeaponsLoading += Events_WeaponsLoading;
            events.PlayerWeaponSwitch += OnPlayerWeaponSwitch;
        }

        public override void GivePlayerWeapons(ITDSPlayer player)
        {
            if (_allRoundWeapons is null)
                return;
            if (Lobby.Gamemodes.CurrentGamemode?.Weapons.HandlesGivingWeapons == true)
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
