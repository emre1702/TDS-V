using GTANetworkAPI;
using TDS_Server.Handler.Events;

namespace TDS_Server.Core.Events
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
