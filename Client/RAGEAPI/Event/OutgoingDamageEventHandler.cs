using System;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Entity;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    public class OutgoingDamageEventHandler : BaseEventHandler<OutgoingDamageDelegate>
    {
        private readonly LoggingHandler _loggingHandler;
        private readonly EntityConvertingHandler _entityConvertingHandler;

        public OutgoingDamageEventHandler(LoggingHandler loggingHandler, EntityConvertingHandler entityConvertingHandler)
            : base()
        {
            _loggingHandler = loggingHandler;
            _entityConvertingHandler = entityConvertingHandler;

            RAGE.Events.OnOutgoingDamage += OutgoingDamage;
        }

        private void OutgoingDamage(RAGE.Elements.Entity sourceEntityMod, RAGE.Elements.Entity targetEntityMod, RAGE.Elements.Player sourcePlayerMod,
            ulong weaponHashMod, ulong boneIdx, int damage, RAGE.Events.CancelEventArgs cancelMod)
        {
            if (Actions.Count == 0)
                return;

            try 
            { 
                IEntity sourceEntity = _entityConvertingHandler.GetEntity(sourceEntityMod);
                IEntity targetEntity = _entityConvertingHandler.GetEntity(targetEntityMod);
                IPlayer sourcePlayer = _entityConvertingHandler.GetEntity(sourcePlayerMod);
            
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

    }
}
