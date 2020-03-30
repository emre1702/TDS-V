using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.ResourceStop)]
        public void ResourceStop()
        {
            Init.TDSCore.EventsHandler.OnResourceStop();
        }
    }
}
