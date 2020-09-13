using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler;

namespace TDS_Server.Core.Damagesystem
{
    public partial class Damagesys
    {
        #region Private Fields

        private readonly ILoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

        public bool DamageDealtThisRound => _allHitters.Count > 0;

        #endregion Public Properties

        #region Public Methods

        public void Clear()
        {
            _allHitters.Clear();
        }

        #endregion Public Methods
    }
}
