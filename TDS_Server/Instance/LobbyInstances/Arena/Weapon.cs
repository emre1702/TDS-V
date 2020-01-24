using GTANetworkAPI;
using System.Linq;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Arena
    {
        public override void GivePlayerWeapons(TDSPlayer player)
        {
            var lastWeapon = player.LastWeaponOnHand;
            player.Player!.RemoveAllWeapons();
            bool giveLastWeapon = false;
            var weapons = LobbyEntity.LobbyWeapons.Where(w => CurrentGameMode != null ? CurrentGameMode.IsWeaponAllowed(w.Hash) : true);
            foreach (LobbyWeapons weapon in weapons)
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