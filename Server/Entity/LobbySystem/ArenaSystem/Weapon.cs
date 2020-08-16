using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.LobbySystem.ArenaSystem
{
    partial class Arena
    {
        #region Private Fields

        private static IEnumerable<LobbyWeapons>? _allRoundWeapons;

        #endregion Private Fields

        #region Public Methods

        public override void GivePlayerWeapons(ITDSPlayer player)
        {
            if (_allRoundWeapons is null)
                return;
            if (CurrentGameMode?.HandlesGivingWeapons == true)
                return;
            var lastWeapon = player.LastWeaponOnHand;
            player.RemoveAllWeapons();
            bool giveLastWeapon = false;

            foreach (LobbyWeapons weapon in _allRoundWeapons)
            {
                //if (!System.Enum.IsDefined(typeof(WeaponHash), (uint) weapon.Hash))
                //    continue;
                player.GiveWeapon((uint)weapon.Hash, weapon.Ammo, false);
                if (weapon.Hash == lastWeapon)
                    giveLastWeapon = true;
            }
            if (giveLastWeapon)
                player.CurrentWeapon = lastWeapon;
        }

        public override void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            base.OnPlayerWeaponSwitch(player, oldWeapon, newWeapon);
            CurrentGameMode?.OnPlayerWeaponSwitch(player, oldWeapon, newWeapon);
        }

        #endregion Public Methods
    }
}
