using GTANetworkAPI;
using System;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Manager.Timer
{
    class TimerManager : Script
    {
        public TimerManager()
        {
            TDSTimer.Init(NAPI.Util.ConsoleOutput, () => (ulong)Environment.TickCount & int.MaxValue);
        }

        [ServerEvent(Event.Update)]
        public static void OnUpdateFunc()
        {
            TDSTimer.OnUpdateFunc();
        }
    }
}