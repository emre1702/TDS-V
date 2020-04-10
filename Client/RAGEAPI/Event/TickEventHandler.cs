using System.Collections.Generic;
using System.Diagnostics;
using TDS_Client.Data.Interfaces.ModAPI.Event;

namespace TDS_Client.RAGEAPI.Event
{
    public class TickEventHandler : BaseEventHandler<TickDelegate>
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public TickEventHandler()
        {
            _stopwatch.Start();

            RAGE.Events.Tick += OnTick;
        }

        private void OnTick(List<RAGE.Events.TickNametagData> nametags)
        {
            ulong currentMs = unchecked((ulong)_stopwatch.ElapsedMilliseconds);
            foreach (var a in Actions)
                if (a.Requirement is null || a.Requirement())
                    a.Method(currentMs);
        }
    }
}
