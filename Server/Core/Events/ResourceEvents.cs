using GTANetworkAPI;
using TDS_Server.Handler.Events;

namespace TDS_Server.Core.Events
{
    public class ResourceEvents : Script
    {
        [ServerEvent(Event.ResourceStop)]
        public void ResourceStop()
        {
            EventsHandler.Instance.OnResourceStop();
        }
    }
}
