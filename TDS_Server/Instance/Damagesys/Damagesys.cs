namespace TDS_Server.Instance
{
    using GTANetworkAPI;
    using System.Collections.Generic;
    using TDS_Server.Dto;
    using TDS_Server.Entity;
    using TDS_Server.Instance.Lobby;

    partial class Damagesys
    {
        public Damagesys(ICollection<LobbyWeapons> weapons)
        {
            foreach (LobbyWeapons weapon in weapons)
            {
                damagesDict[(WeaponHash)weapon.Hash] = new DamageDto(weapon);
            }
        }

        public void Clear()
        {
            allHitters.Clear();
        }
    }

}
