namespace TDS.Instance.Lobby
{

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS.Entity;

    partial class FightLobby
    {
        public void GivePlayerWeapons(Client player)
        {
            player.RemoveAllWeapons();
            foreach (LobbyWeapons weapon in LobbyEntity.LobbyWeapons)
            {
                if (!System.Enum.IsDefined(typeof(WeaponHash), weapon.Hash))
                    continue;
                WeaponHash hash = (WeaponHash)weapon.Hash;
                player.GiveWeapon(hash, 0);
                player.SetWeaponAmmo(hash, (int)weapon.Ammo);
            }
        }
    }

}
