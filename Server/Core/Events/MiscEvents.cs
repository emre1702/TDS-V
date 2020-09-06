using GTANetworkAPI;
using TDS_Server.Handler.Events;

namespace TDS_Server.Core.Events
{
    public class MiscEvents : Script
    {
        [ServerEvent(Event.Update)]
        public void Update()
        {
            EventsHandler.Instance.OnUpdate();
        }
    }
}
