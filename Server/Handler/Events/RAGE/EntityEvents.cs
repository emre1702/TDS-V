using GTANetworkAPI;

namespace TDS_Server.Handler.Events.RAGE
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
