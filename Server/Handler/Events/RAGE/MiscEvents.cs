using GTANetworkAPI;

namespace TDS_Server.Handler.Events.RAGE
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
