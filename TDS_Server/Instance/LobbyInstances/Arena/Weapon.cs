using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Arena
    {
        private static IEnumerable<LobbyWeapons>? _allRoundWeapons;

        public override void GivePlayerWeapons(TDSPlayer player)
        {
            if (_allRoundWeapons is null)
                return;
            var lastWeapon = player.LastWeaponOnHand;
            player.Player!.RemoveAllWeapons();
            bool giveLastWeapon = false;
            
            foreach (LobbyWeapons weapon in _allRoundWeapons)
            {
                //if (!System.Enum.IsDefined(typeof(WeaponHash), (uint) weapon.Hash))
                //    continue;
                WeaponHash hash = (WeaponHash)((uint)weapon.Hash);
                NAPI.Player.GivePlayerWeapon(player.Player, hash, 0);
                NAPI.Player.SetPlayerWeaponAmmo(player.Player, hash, weapon.Ammo);
                if (hash == lastWeapon)
                    giveLastWeapon = true;
            }
            if (giveLastWeapon)
                NAPI.Player.SetPlayerCurrentWeapon(player.Player, lastWeapon);
        }

        public override void OnPlayerWeaponSwitch(TDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon)
        {
            base.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
            CurrentGameMode?.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
        }
    }
}
