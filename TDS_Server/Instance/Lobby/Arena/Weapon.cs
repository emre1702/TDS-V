using GTANetworkAPI;
using System.Linq;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity.Lobby;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        public override void GivePlayerWeapons(TDSPlayer player)
        {
            var lastWeapon = player.LastWeaponOnHand;
            player.Client.RemoveAllWeapons();
            bool giveLastWeapon = false;
            var weapons = LobbyEntity.LobbyWeapons.Where(w => CurrentGameMode != null ? CurrentGameMode.IsWeaponAllowed(w.Hash) : true);
            foreach (LobbyWeapons weapon in weapons)
            {
                //if (!System.Enum.IsDefined(typeof(WeaponHash), (uint) weapon.Hash))
                //    continue;
                player.GiveWeapon(weapon.Hash, weapon.Ammo);
                if ((uint)weapon.Hash == (uint)lastWeapon)
                    giveLastWeapon = true;
            }
            if (giveLastWeapon)
                NAPI.Player.SetPlayerCurrentWeapon(player.Client, lastWeapon);

            if (player.WeaponUpgradesDatasJsonComplete is { })
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SyncMyWeaponUpgrades, player.WeaponUpgradesDatasJsonComplete);
        }

        public override void OnPlayerWeaponSwitch(TDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon)
        {
            base.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
            CurrentGameMode?.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
        }
    }
}