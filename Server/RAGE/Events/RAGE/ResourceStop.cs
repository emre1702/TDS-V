using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.RAGE.Startup;

namespace TDS_Server.RAGE.Events.RAGE
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
