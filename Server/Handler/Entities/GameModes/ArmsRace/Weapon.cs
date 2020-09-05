using GTANetworkAPI;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class ArmsRace
    {
        #region Private Fields

        private Dictionary<short, LobbyArmsRaceWeapons> _weapons = new Dictionary<short, LobbyArmsRaceWeapons>();

        #endregion Private Fields

        #region Public Properties

        public override bool HandlesGivingWeapons => true;

        #endregion Public Properties

        #region Public Methods

        public override void GivePlayerWeapons(ITDSPlayer player)
        {
            var weapon = GetCurrentWeapon(player);
            player.RemoveAllWeapons();
            player.GiveWeapon(weapon, 9999);
        }

        public override bool IsWeaponAllowed(WeaponHash weaponHash)
                    => Lobby.Entity.ArmsRaceWeapons.Any(w => w.WeaponHash == weaponHash);

        #endregion Public Methods

        #region Private Methods

        private WeaponHash GetCurrentWeapon(ITDSPlayer player)
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

        private bool GetNextWeapon(ITDSPlayer player, out WeaponHash? weaponHash)
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

            player.RemoveAllWeapons();
            player.GiveWeapon(weaponHash.Value, 9999);
            player.CurrentWeapon = weaponHash.Value;
        }

        private void LoadWeapons()
        {
            _weapons = Lobby.Entity.ArmsRaceWeapons.ToDictionary(a => a.AtKill, a => a);
        }

        #endregion Private Methods
    }
}
