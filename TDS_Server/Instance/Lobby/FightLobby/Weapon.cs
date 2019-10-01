using GTANetworkAPI;
using Newtonsoft.Json;
using System.Linq;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity.Lobby;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        public virtual void GivePlayerWeapons(TDSPlayer player)
        {
            var lastWeapon = player.LastWeaponOnHand;
            player.Client.RemoveAllWeapons();
            bool giveLastWeapon = false;
            foreach (LobbyWeapons weapon in LobbyEntity.LobbyWeapons)
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
    }
}