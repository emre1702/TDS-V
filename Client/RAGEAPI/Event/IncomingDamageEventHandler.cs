using TDS_Shared.Data.Models;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.RAGEAPI.Entity;
using TDS_Shared.Data.Enums;
using TDS_Client.Handler;
using System;

namespace TDS_Client.RAGEAPI.Event
{
    public class IncomingDamageEventHandler : BaseEventHandler<IncomingDamageDelegate>
    {
        private readonly LoggingHandler _loggingHandler;
        private readonly EntityConvertingHandler _entityConvertingHandler;

        public IncomingDamageEventHandler(LoggingHandler loggingHandler, EntityConvertingHandler entityConvertingHandler)
            : base()
        {
            _loggingHandler = loggingHandler;
            _entityConvertingHandler = entityConvertingHandler;

            RAGE.Events.OnIncomingDamage += IncomingDamage;
        }

        private void IncomingDamage(RAGE.Elements.Player sourcePlayerMod, RAGE.Elements.Entity sourceEntityMod, RAGE.Elements.Entity targetEntityMod, 
            ulong weaponHashMod, ulong boneIdx, int damage, RAGE.Events.CancelEventArgs cancelMod)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                IPlayer sourcePlayer = _entityConvertingHandler.GetEntity(sourcePlayerMod);
                IEntity sourceEntity = _entityConvertingHandler.GetEntity(sourceEntityMod);
                IEntity targetEntity = _entityConvertingHandler.GetEntity(targetEntityMod);
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

    }
}
