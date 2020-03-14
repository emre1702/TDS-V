using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS_Shared.Data.Enums;
using TDS_Server.Dto;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Core.Damagesystem
{
    partial class Damagesys
    {
        public bool DamageDealtThisRound => _allHitters.Count > 0;

        #nullable disable warnings
        private static Dictionary<WeaponHash, DamageDto> _defaultDamages;
        #nullable restore warnings

        public Damagesys(ICollection<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards)
        {
            foreach (LobbyWeapons weapon in weapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    _damagesDict[weapon.Hash] = _defaultDamages[weapon.Hash];
                else
                    _damagesDict[weapon.Hash] = new DamageDto(weapon);
            }
            InitKillingSpreeRewards(killingspreeRewards);
        }

        public void Clear()
        {
            _allHitters.Clear();
        }

        public static void LoadDefaults(TDSDbContext dbcontext)
        {
            _defaultDamages = dbcontext.Weapons
                .ToDictionary(
                    w => w.Hash,
                    w => new DamageDto
                    {
                        Damage = w.Damage,
                        HeadMultiplier = w.HeadShotDamageModifier
                    }
                );
        }
    }
}
