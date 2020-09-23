using System.Collections;
using System.Collections.Generic;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.LobbyEntities;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Weapons
{
    public class FightLobbyWeapons
    {
        private readonly LobbyDb _entity;

        public FightLobbyWeapons(LobbyDb entity)
        {
            _entity = entity;
        }

        public virtual void GivePlayerWeapons(ITDSPlayer player)
        {
            var lastWeapon = player.LastWeaponOnHand;
            player.RemoveAllWeapons();
            bool giveLastWeapon = false;
            foreach (var weapon in GetAllWeapons())
            {
                player.GiveWeapon(weapon.Hash, 0);
                player.SetWeaponAmmo(weapon.Hash, weapon.Ammo);
                if (weapon.Hash == lastWeapon)
                    giveLastWeapon = true;
            }
            if (giveLastWeapon)
                player.CurrentWeapon = lastWeapon;
        }

        internal virtual IEnumerable<LobbyWeapons> GetAllWeapons()
            => _entity.LobbyWeapons;
    }
}
