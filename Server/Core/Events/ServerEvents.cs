using GTANetworkAPI;
using System;
using TDS.Server.Handler.Events;

namespace TDS.Server.Core.Events
{
    public class ServerEvents : Script
    {
        [ServerEvent(Event.UnhandledException)]
        public void OnUnhandledException(Exception ex)
        {
            EventsHandler.Instance.OnError(ex, "Unhandled exception: ");
        }
    }
}
