using GTANetworkAPI;
using System;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;

namespace TDS.Server.Core.Events
{
    public class EntityEvents : Script
    {
        [ServerEvent(Event.EntityDeleted)]
        public void EntityDeleted(Entity entity)
        {
            try
            { 
                EventsHandler.Instance.OnEntityDeleted(entity);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }
    }
}
