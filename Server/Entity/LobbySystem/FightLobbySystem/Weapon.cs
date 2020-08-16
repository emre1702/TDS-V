using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Entity.LobbySystem.FightLobbySystem
{
    partial class FightLobby
    {
        #region Public Methods

        public virtual void GivePlayerWeapons(ITDSPlayer player)
        {
            var lastWeapon = player.LastWeaponOnHand;
            player.RemoveAllWeapons();

            bool giveLastWeapon = false;
            foreach (LobbyWeapons weapon in Entity.LobbyWeapons)
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

        #endregion Public Methods
    }
}
