using System;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    public class IncomingDamageEventHandler : BaseEventHandler<IncomingDamageDelegate>
    {
        #region Private Fields

        private readonly LoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public IncomingDamageEventHandler(LoggingHandler loggingHandler)
            : base()
        {
            _loggingHandler = loggingHandler;

            RAGE.Events.OnIncomingDamage += IncomingDamage;
        }

        #endregion Public Constructors

        #region Private Methods

        private void IncomingDamage(RAGE.Elements.Player sourcePlayerMod, RAGE.Elements.Entity sourceEntityMod, RAGE.Elements.Entity targetEntityMod,
            ulong weaponHashMod, ulong boneIdx, int damage, RAGE.Events.CancelEventArgs cancelMod)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                IPlayer sourcePlayer = sourcePlayerMod as IPlayer;
                IEntity sourceEntity = sourceEntityMod as IEntity;
                IEntity targetEntity = targetEntityMod as IEntity;
                var weaponHash = (WeaponHash)weaponHashMod;
                var cancel = new CancelEventArgs();

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(sourcePlayer, sourceEntity, targetEntity, weaponHash, boneIdx, damage, cancel);
                }

                cancelMod.Cancel = cancel.Cancel;
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        #endregion Private Methods
    }
}
