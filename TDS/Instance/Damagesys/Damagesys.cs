namespace TDS.Instance
{
    using GTANetworkAPI;
    using System.Collections.Generic;
    using TDS.Dto;
    using TDS.Entity;
    using TDS.Instance.Lobby;

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
            lastHittersDict.Clear();
        }
    }

}
