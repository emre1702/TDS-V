using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.Weapons
{
    public class RoundFightLobbyWeapons : FightLobbyWeapons, IRoundFightLobbyWeapons
    {
        private List<LobbyWeapons> _allRoundWeapons = new List<LobbyWeapons>();
        private readonly RoundFightLobby _lobby;

        public RoundFightLobbyWeapons(RoundFightLobby lobby) : base(lobby.Entity)
        {
            _lobby = lobby;

            lobby.Events.WeaponsLoading += Events_WeaponsLoading;
        }

        public override void GivePlayerWeapons(ITDSPlayer player)
        {
            if (_allRoundWeapons is null)
                return;
            if (_lobby.Gamemodes.CurrentGamemode?.HandlesGivingWeapons == true)
                return;
            base.GivePlayerWeapons(player);
        }

        internal void SetRoundWeapons(List<LobbyWeapons> weapons)
            => _allRoundWeapons = weapons;

        internal override IEnumerable<LobbyWeapons> GetAllWeapons()
            => _allRoundWeapons;

        public virtual void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            _lobby.Gamemodes.CurrentGamemode?.OnPlayerWeaponSwitch(player, oldWeapon, newWeapon);
        }

        private void Events_WeaponsLoading()
        {
            _allRoundWeapons = _lobby.Entity.LobbyWeapons
                .Where(w => _lobby.Rounds.CurrentGamemode?.IsWeaponAllowed(w.Hash) != false)
                .ToList();
        }
    }
}
