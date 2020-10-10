using GTANetworkAPI;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.GamemodesSystem.Weapons
{
    public class ArmsRaceWeapons : BaseGamemodeWeapons, IArmsRaceGamemodeWeapons
    {
        public override bool HandlesGivingWeapons => true;

        private Dictionary<short, LobbyArmsRaceWeapons> _weapons = new Dictionary<short, LobbyArmsRaceWeapons>();

        private readonly IRoundFightLobby _lobby;
        private readonly IArmsRaceGamemode _gamemode;

        public ArmsRaceWeapons(IRoundFightLobby lobby, IArmsRaceGamemode gamemode)
        {
            _lobby = lobby;
            _gamemode = gamemode;

            InitWeaponsDict();
        }

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);

            events.PlayerDied += Events_PlayerDied;
            events.PlayerSpawned += Events_PlayerSpawned;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);

            events.PlayerDied -= Events_PlayerDied;
            events.PlayerSpawned -= Events_PlayerSpawned;
        }

        private void Events_PlayerDied(ITDSPlayer player, ITDSPlayer killer, uint weapon, int hadLifes)
        {
            if (player == killer)
                return;
            if (_gamemode.Rounds.CheckHasKillerWonTheRound(killer))
                return;

            GiveNextWeapon(player);
        }

        private void Events_PlayerSpawned(ITDSPlayer player)
            => GivePlayerWeapon(player);

        public override bool IsWeaponAllowed(WeaponHash weaponHash)
            => _lobby.Entity.ArmsRaceWeapons.Any(w => w.WeaponHash == weaponHash);

        private void GivePlayerWeapon(ITDSPlayer player, WeaponHash? weapon = null)
        {
            weapon ??= GetCurrentWeapon(player);
            NAPI.Task.Run(() =>
            {
                player.RemoveAllWeapons();
                player.GiveWeapon(weapon.Value, 9999);
                player.CurrentWeapon = weapon.Value;
            });
        }

        public WeaponHash GetCurrentWeapon(ITDSPlayer player)
        {
            if (player.CurrentRoundStats is null)
                return WeaponHash.Unarmed;
            if (_weapons.TryGetValue((short)player.CurrentRoundStats.Kills, out LobbyArmsRaceWeapons? info) && info.WeaponHash.HasValue)
                return info.WeaponHash.Value;

            //Todo: Make sure there is always a weapon at AtKills = 0
            return _weapons
                .Where(w => w.Key <= player.CurrentRoundStats.Kills && w.Value.WeaponHash.HasValue)
                .MaxBy(w => w.Key)
                .Select(w => w.Value)
                .FirstOrDefault()?
                .WeaponHash ?? WeaponHash.Unarmed;
        }

        public bool GetNextWeapon(ITDSPlayer player, out WeaponHash? weaponHash)
        {
            var kills = player.CurrentRoundStats?.Kills;
            if (!kills.HasValue)
            {
                weaponHash = null;
                return false;
            }

            if (!_weapons.TryGetValue((short)kills.Value, out LobbyArmsRaceWeapons? info))
            {
                weaponHash = null;
                return false;
            }

            weaponHash = info.WeaponHash;
            return info.WeaponHash != null;
        }

        private void GiveNextWeapon(ITDSPlayer player)
        {
            if (!GetNextWeapon(player, out WeaponHash? weaponHash) || !weaponHash.HasValue)
                return;

            GivePlayerWeapon(player, weaponHash.Value);
        }

        private void InitWeaponsDict()
        {
            _weapons = _lobby.Entity.ArmsRaceWeapons.ToDictionary(a => a.AtKill, a => a);
        }
    }
}
