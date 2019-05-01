namespace TDS_Server.Instance.Lobby
{
    using GTANetworkAPI;
    using TDS_Server.Entity;

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