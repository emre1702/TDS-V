using TDS_Shared.Data.Models;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.RAGEAPI.Entity;
using TDS_Shared.Data.Enums;

namespace TDS_Client.RAGEAPI.Event
{
    public class IncomingDamageEventHandler : BaseEventHandler<IncomingDamageDelegate>
    {
        private readonly EntityConvertingHandler _entityConvertingHandler;

        public IncomingDamageEventHandler(EntityConvertingHandler entityConvertingHandler)
            : base()
        {
            _entityConvertingHandler = entityConvertingHandler;

            RAGE.Events.OnIncomingDamage += IncomingDamage;
        }

        private void IncomingDamage(RAGE.Elements.Player sourcePlayerMod, RAGE.Elements.Entity sourceEntityMod, RAGE.Elements.Entity targetEntityMod, 
            ulong weaponHashMod, ulong boneIdx, int damage, RAGE.Events.CancelEventArgs cancelMod)
        {
            if (Actions.Count == 0)
                return;

            IPlayer sourcePlayer = _entityConvertingHandler.GetEntity(sourcePlayerMod);
            IEntity sourceEntity = _entityConvertingHandler.GetEntity(sourceEntityMod);
            IEntity targetEntity = _entityConvertingHandler.GetEntity(targetEntityMod);
            var weaponHash = (WeaponHash)weaponHashMod;
            var cancel = new CancelEventArgs();

            foreach (var action in Actions)
                if (action.Requirement is null || action.Requirement())
                    action.Method(sourcePlayer, sourceEntity, targetEntity, weaponHash, boneIdx, damage, cancel);

            cancelMod.Cancel = cancel.Cancel;
        }

    }
}
