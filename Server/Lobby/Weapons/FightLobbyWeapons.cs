using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.LobbySystem.Weapons
{
    public class FightLobbyWeapons : IFightLobbyWeapons
    {
        protected readonly IFightLobby Lobby;

        public FightLobbyWeapons(IFightLobby lobby)
        {
            Lobby = lobby;
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
            => Lobby.Entity.LobbyWeapons;
    }
}
