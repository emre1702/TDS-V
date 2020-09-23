using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        private static IEnumerable<LobbyWeapons>? _allRoundWeapons;

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
                WeaponHash hash = (WeaponHash)((uint)weapon.Hash);
                player.GiveWeapon(hash, 0);
                player.SetWeaponAmmo(hash, weapon.Ammo);
                if (hash == lastWeapon)
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
    }
}
