using GTANetworkAPI;
using System;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Manager.Utility
{
    internal class TimerManager : Script
    {
        public TimerManager()
        {
            TDSTimer.Init(NAPI.Util.ConsoleOutput, () => (ulong)Environment.TickCount & int.MaxValue);
            new TDSTimer(MinuteTimer.Execute, 60 * 1000, 0);
            new TDSTimer(HourTimer.Execute, 60 * 60 * 1000, 0);
        }

        [ServerEvent(Event.Update)]
        public static void OnUpdateFunc()
        {
            TDSTimer.OnUpdateFunc();
        }
    }
}