using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler;

namespace TDS_Server.Core.Damagesystem
{
    public partial class Damagesys
    {
        private readonly ILoggingHandler _loggingHandler;
        private readonly WeaponDatasLoadingHandler _weaponDatasLoadingHandler;

        public Damagesys(IEnumerable<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards,
            ILoggingHandler loggingHandler, WeaponDatasLoadingHandler weaponDatasLoadingHandler)
        {
            _loggingHandler = loggingHandler;

            foreach (LobbyWeapons weapon in weapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    _damagesDict[weapon.Hash] = weaponDatasLoadingHandler.DefaultDamages[weapon.Hash];
                else
                    _damagesDict[weapon.Hash] = new DamageDto(weapon);
            }
            InitKillingSpreeRewards(killingspreeRewards);
        }

        public Damagesys(ILoggingHandler loggingHandler, WeaponDatasLoadingHandler weaponDatasLoadingHandler)
        {
            _loggingHandler = loggingHandler;
            _weaponDatasLoadingHandler = weaponDatasLoadingHandler;
        }

        public void Init(IEnumerable<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards)
        {
            foreach (var weapon in weapons)
            {
                if (!weapon.Damage.HasValue && !weapon.HeadMultiplicator.HasValue)
                    _damagesDict[weapon.Hash] = _weaponDatasLoadingHandler.DefaultDamages[weapon.Hash];
                else
                    _damagesDict[weapon.Hash] = new DamageDto(weapon);
            }
            InitKillingSpreeRewards(killingspreeRewards);
        }

        public bool DamageDealtThisRound => _allHitters.Count > 0;

        public void Clear()
        {
            _allHitters.Clear();
        }
    }
}
