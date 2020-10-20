using GTANetworkAPI;
using System;
using TDS_Server.Handler;
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

        [ServerEvent(Event.UnhandledException)]
        public void UnhandledException(Exception ex)
        {
            LoggingHandler.Instance?.LogError(ex);
        }
    }
}
