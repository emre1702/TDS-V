using GTANetworkAPI;
using System.Linq;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server.Core.Instance.LobbyInstances.FightLobby
{
    partial class FightLobby
    {
        public virtual void GivePlayerWeapons(TDSPlayer player)
        {
            var lastWeapon = player.LastWeaponOnHand;
            player.Player!.RemoveAllWeapons();
            bool giveLastWeapon = false;
            foreach (LobbyWeapons weapon in LobbyEntity.LobbyWeapons)
            {
                //if (!System.Enum.IsDefined(typeof(WeaponHash), (uint) weapon.Hash))
                //    continue;
                WeaponHash hash = (WeaponHash) ((uint)weapon.Hash);
                NAPI.Player.GivePlayerWeapon(player.Player, hash, 0);
                NAPI.Player.SetPlayerWeaponAmmo(player.Player, hash, weapon.Ammo);
                if (hash == lastWeapon)
                    giveLastWeapon = true;
            }
            if (giveLastWeapon)
                NAPI.Player.SetPlayerCurrentWeapon(player.Player, lastWeapon);
        }
    }
}