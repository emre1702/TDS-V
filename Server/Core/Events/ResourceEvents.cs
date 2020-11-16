using GTANetworkAPI;
using TDS.Server.Handler.Events;

namespace TDS.Server.Core.Events
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
