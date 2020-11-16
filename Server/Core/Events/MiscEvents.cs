using GTANetworkAPI;
using System;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;

namespace TDS.Server.Core.Events
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
