using GTANetworkAPI;
using TDS_Server_DB.Entity.Lobby;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        public void GivePlayerWeapons(Client player)
        {
            player.RemoveAllWeapons();
            foreach (LobbyWeapons weapon in LobbyEntity.LobbyWeapons)
            {
                //if (!System.Enum.IsDefined(typeof(WeaponHash), (uint) weapon.Hash))
                //    continue;
                WeaponHash hash = (WeaponHash) ((uint)weapon.Hash);
                player.GiveWeapon(hash, 0);
                player.SetWeaponAmmo(hash, weapon.Ammo);
            }
        }
    }
}