using GTANetworkAPI;
using System;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.UnhandledException)]
        public void OnUnhandledException(Exception ex)
        {
            Init.TDSCore.HandleProgramException(ex, "Unhandled exception: ");
        }
    }
}
