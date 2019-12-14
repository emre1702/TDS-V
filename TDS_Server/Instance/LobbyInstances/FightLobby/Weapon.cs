using GTANetworkAPI;
using System.Linq;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server.Instance.LobbyInstances
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
                WeaponHash hash = (WeaponHash) ((uint)weapon.Hash);
                NAPI.Player.GivePlayerWeapon(player.Client, hash, 0);
                NAPI.Player.SetPlayerWeaponAmmo(player.Client, hash, weapon.Ammo);
                if (hash == lastWeapon)
                    giveLastWeapon = true;
            }
            if (giveLastWeapon)
                NAPI.Player.SetPlayerCurrentWeapon(player.Client, lastWeapon);
        }
    }
}