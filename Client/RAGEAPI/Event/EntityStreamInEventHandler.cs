using System;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Event
{
    public class EntityStreamInEventHandler : BaseEventHandler<EntityStreamInDelegate>
    {
        #region Private Fields

        private readonly EntityConvertingHandler _entityConvertingHandler;
        private readonly LoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public EntityStreamInEventHandler(LoggingHandler loggingHandler, EntityConvertingHandler entityConvertingHandler)
            : base()
        {
            _loggingHandler = loggingHandler;
            _entityConvertingHandler = entityConvertingHandler;

            RAGE.Events.OnEntityStreamIn += EntityStreamIn;
        }

        #endregion Public Constructors

        #region Private Methods

        private void EntityStreamIn(RAGE.Elements.Entity modEntity)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                IEntity entity = _entityConvertingHandler.GetEntity(modEntity);

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(entity);
                }
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        #endregion Private Methods
    }
}
