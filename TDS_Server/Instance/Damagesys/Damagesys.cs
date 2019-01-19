namespace TDS_Server.Instance
{
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using TDS_Server.Dto;
    using TDS_Server.Entity;
    using TDS_Server.Instance.Lobby;

    partial class Damagesys
    {
        private static Dictionary<uint, DamageDto> defaultDamages;

        public Damagesys(ICollection<LobbyWeapons> weapons)
        {
            foreach (LobbyWeapons weapon in weapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    damagesDict[(WeaponHash)weapon.Hash] = defaultDamages[weapon.Hash];
                else
                    damagesDict[(WeaponHash)weapon.Hash] = new DamageDto(weapon);
            }
        }

        public void Clear()
        {
            allHitters.Clear();
        }

        public static void LoadDefaults(TDSNewContext dbcontext)
        {
            defaultDamages = dbcontext.Weapons.Select(w => new { w.Hash, w.DefaultDamage, w.DefaultHeadMultiplicator }).ToDictionary(
                w => w.Hash, w => new DamageDto { Damage = w.DefaultDamage, HeadMultiplier = w.DefaultHeadMultiplicator }
            );
        }
    }

}
