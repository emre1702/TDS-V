using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Event
{
    public class EntityStreamInEventHandler : BaseEventHandler<EntityStreamInDelegate>
    {
        private readonly EntityConvertingHandler _entityConvertingHandler;

        public EntityStreamInEventHandler(EntityConvertingHandler entityConvertingHandler)
            : base()
        {
            _entityConvertingHandler = entityConvertingHandler;

            RAGE.Events.OnEntityStreamIn += EntityStreamIn;
        }

        private void EntityStreamIn(RAGE.Elements.Entity modEntity)
        {
            if (Actions.Count == 0)
                return;

            IEntity entity = _entityConvertingHandler.GetEntity(modEntity);

            foreach (var action in Actions)
                if (action.Requirement is null || action.Requirement())
                    action.Method(entity);
        }

    }
}
