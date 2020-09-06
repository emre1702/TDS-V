using GTANetworkAPI;

namespace TDS_Server.Handler.Events.RAGE
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
