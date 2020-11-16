using GTANetworkAPI;
using TDS.Server.Handler.Events;

namespace TDS.Server.Core.Events
{
    public class EntityEvents : Script
    {
        [ServerEvent(Event.EntityDeleted)]
        public void EntityDeleted(Entity entity)
        {
            EventsHandler.Instance.OnEntityDeleted(entity);
        }
    }
}
