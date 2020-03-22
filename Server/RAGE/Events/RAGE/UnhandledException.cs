using GTANetworkAPI;
using System;
using TDS_Server.RAGE.Startup;

namespace TDS_Server.RAGE.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.UnhandledException)]
        public void OnUnhandledException(Exception ex)
        {
            Program.TDSCore.HandleProgramException(ex, "Unhandled exception: ");
        }
    }
}
