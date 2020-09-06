using GTANetworkAPI;
using System;
using TDS_Server.Handler.Events;

namespace TDS_Server.Core.Events
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
