using GTANetworkAPI;
using TDS_Server.RAGE.Startup;

namespace TDS_Server.RAGE.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.Update)]
        public void OnUpdate()
        {
            Program.TDSCore.EventsHandler.OnUpdate();
        }
    }
}
