using System;
using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Shared.Core;

namespace TDS_Client.RAGEAPI.Event
{
    public class TickEventHandler : BaseEventHandler<TickDelegate>
    {
        public TickEventHandler()
        {
            RAGE.Events.Tick += OnTick;
        }

        private void OnTick(List<RAGE.Events.TickNametagData> nametags)
        {
            int currentMs = TDSTimer.ElapsedMs;
            foreach (var a in Actions)
                if (a.Requirement is null || a.Requirement())
                    a.Method(currentMs);
        }
    }
}
