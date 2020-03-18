using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class FightLobby
    {
        public virtual void GivePlayerWeapons(ITDSPlayer player)
        {
            var lastWeapon = player.LastWeaponOnHand;
            player.ModPlayer!.RemoveAllWeapons();
            bool giveLastWeapon = false;
            foreach (LobbyWeapons weapon in Entity.LobbyWeapons)
            {
                //if (!System.Enum.IsDefined(typeof(WeaponHash), (uint) weapon.Hash))
                //    continue;
                player.ModPlayer.GiveWeapon(weapon.Hash);
                player.ModPlayer.SetWeaponAmmo(weapon.Hash, weapon.Ammo);
                if (weapon.Hash == lastWeapon)
                    giveLastWeapon = true;
            }
            if (giveLastWeapon)
                player.ModPlayer.CurrentWeapon = lastWeapon;
        }
    }
}
