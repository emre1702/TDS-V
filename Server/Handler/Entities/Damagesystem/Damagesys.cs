using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Core.Damagesystem
{
    public partial class Damagesys
    {
        public bool DamageDealtThisRound => _allHitters.Count > 0;

#nullable disable warnings
        private static Dictionary<WeaponHash, DamageDto> _defaultDamages;
#nullable restore warnings

        private readonly IModAPI _modAPI;
        private readonly LoggingHandler _loggingHandler;

        public Damagesys(ICollection<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards, IModAPI modAPI, LoggingHandler loggingHandler)
        {
            _modAPI = modAPI;
            _loggingHandler = loggingHandler;

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
