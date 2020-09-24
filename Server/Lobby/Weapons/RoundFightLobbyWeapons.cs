using System.Collections.Generic;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.Weapons
{
    public class RoundFightLobbyWeapons : FightLobbyWeapons
    {
        private List<LobbyWeapons> _allRoundWeapons = new List<LobbyWeapons>();
        private readonly RoundFightLobby _lobby;

        public RoundFightLobbyWeapons(RoundFightLobby lobby) : base(lobby.Entity)
        {
            _lobby = lobby;
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
    }
}
