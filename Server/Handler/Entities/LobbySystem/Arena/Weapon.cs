using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.LobbySystem
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
            var lastWeapon = player.LastWeaponOnHand;
            player.ModPlayer!.RemoveAllWeapons();
            bool giveLastWeapon = false;

            foreach (LobbyWeapons weapon in _allRoundWeapons)
            {
                //if (!System.Enum.IsDefined(typeof(WeaponHash), (uint) weapon.Hash))
                //    continue;
                WeaponHash hash = (WeaponHash)((uint)weapon.Hash);
                player.ModPlayer.GiveWeapon(hash);
                player.ModPlayer.SetWeaponAmmo(hash, weapon.Ammo);
                if (hash == lastWeapon)
                    giveLastWeapon = true;
            }
            if (giveLastWeapon)
                player.ModPlayer.CurrentWeapon = lastWeapon;
        }

        public override void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            base.OnPlayerWeaponSwitch(player, oldWeapon, newWeapon);
            CurrentGameMode?.OnPlayerWeaponSwitch(player, oldWeapon, newWeapon);
        }

        #endregion Public Methods
    }
}
