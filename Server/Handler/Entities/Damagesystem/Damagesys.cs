using System.Collections.Generic;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler.Helper;

namespace TDS_Server.Core.Damagesystem
{
    public partial class Damagesys
    {
        #region Private Fields

        private readonly LangHelper _langHelper;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;

        #endregion Private Fields

        #region Public Constructors

        public Damagesys(ICollection<LobbyWeapons> weapons, ICollection<LobbyKillingspreeRewards> killingspreeRewards, IModAPI modAPI, ILoggingHandler loggingHandler,
            WeaponDatasLoadingHandler weaponDatasLoadingHandler, LangHelper langHelper)
        {
            _modAPI = modAPI;
            _loggingHandler = loggingHandler;
            _langHelper = langHelper;

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
