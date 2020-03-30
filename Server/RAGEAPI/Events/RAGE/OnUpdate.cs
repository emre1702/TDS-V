using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.Update)]
        public void OnUpdate()
        {
            Init.TDSCore.EventsHandler.OnUpdate();
        }
    }
}
