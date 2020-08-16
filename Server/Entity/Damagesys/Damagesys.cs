using System.Collections.Generic;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Entity.Damagesys
{
    public partial class DamageSystem : IDamageSystem
    {
        #region Fields

        private readonly ILoggingHandler _loggingHandler;

        #endregion Fields

        #region Constructors

        public DamageSystem(IEnumerable<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards,
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

        #endregion Constructors

        #region Properties

        public bool DamageDealtThisRound => _allHitters.Count > 0;

        #endregion Properties

        #region Methods

        public void Clear()
        {
            _allHitters.Clear();
        }

        #endregion Methods
    }
}
