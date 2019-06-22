using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance
{
    partial class Damagesys
    {
        private static Dictionary<EWeaponHash, DamageDto> defaultDamages;

        public Damagesys(ICollection<LobbyWeapons> weapons)
        {
            foreach (LobbyWeapons weapon in weapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    damagesDict[weapon.Hash] = defaultDamages[weapon.Hash];
                else
                    damagesDict[weapon.Hash] = new DamageDto(weapon);
            }
        }

        public void Clear()
        {
            allHitters.Clear();
        }

        public static void LoadDefaults(TDSNewContext dbcontext)
        {
            defaultDamages = dbcontext.Weapons
                .ToDictionary(
                    w => w.Hash,
                    w => new DamageDto
                    {
                        Damage = w.DefaultDamage,
                        HeadMultiplier = w.DefaultHeadMultiplicator
                    }
                );
        }
    }
}