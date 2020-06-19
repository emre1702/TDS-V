using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.EntityDeleted)]
        public void EntityDeleted(GTANetworkAPI.Entity modEntity)
        {
            if (!(modEntity is IEntity entity))
                return;
            Init.TDSCore.EventsHandler.OnEntityDeleted(entity);
        }

        #endregion Public Methods
    }
}
