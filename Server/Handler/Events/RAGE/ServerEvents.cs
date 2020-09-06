using GTANetworkAPI;
using System;

namespace TDS_Server.Handler.Events.RAGE
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
