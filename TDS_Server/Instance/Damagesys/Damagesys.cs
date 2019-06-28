using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Lobby;

namespace TDS_Server.Instance
{
    partial class Damagesys
    {
        private static Dictionary<EWeaponHash, DamageDto> _defaultDamages;

        public Damagesys(ICollection<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards)
        {
            foreach (LobbyWeapons weapon in weapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    damagesDict[weapon.Hash] = _defaultDamages[weapon.Hash];
                else
                    damagesDict[weapon.Hash] = new DamageDto(weapon);
            }
            InitKillingSpreeRewards(killingspreeRewards);
        }

        public void Clear()
        {
            allHitters.Clear();
        }

        public static void LoadDefaults(TDSNewContext dbcontext)
        {
            _defaultDamages = dbcontext.Weapons
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