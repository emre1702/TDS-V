using System;
using TDS_Client.Data.Interfaces.RAGE.Game.Entity;
using TDS_Client.Data.Interfaces.RAGE.Game.Event;
using TDS_Client.Data.Interfaces.RAGE.Game.Player;
using TDS_Client.Handler;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    public class OutgoingDamageEventHandler : BaseEventHandler<OutgoingDamageDelegate>
    {
        #region Private Fields

        private readonly LoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public OutgoingDamageEventHandler(LoggingHandler loggingHandler)
            : base()
        {
            _loggingHandler = loggingHandler;

            RAGE.Events.OnOutgoingDamage += OutgoingDamage;
        }

        #endregion Public Constructors

        #region Private Methods

        private void OutgoingDamage(RAGE.Elements.Entity sourceEntityMod, RAGE.Elements.Entity targetEntityMod, RAGE.Elements.Player sourcePlayerMod,
            ulong weaponHashMod, ulong boneIdx, int damage, RAGE.Events.CancelEventArgs cancelMod)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                IEntity sourceEntity = sourceEntityMod as IEntity;
                IEntity targetEntity = targetEntityMod as IEntity;
                ITDSPlayer sourcePlayer = sourcePlayerMod as ITDSPlayer;

                var weaponHash = (WeaponHash)weaponHashMod;
                var cancel = new CancelEventArgs();

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(sourceEntity, targetEntity, sourcePlayer, weaponHash, boneIdx, damage, cancel);
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