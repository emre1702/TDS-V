using GTANetworkAPI;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Manager.Utility
{
    class TimerManager : Script
    {
        public TimerManager()
        {
            TDSTimer.Init(NAPI.Util.ConsoleOutput);
        }

        [ServerEvent(Event.Update)]
        public static void OnUpdateFunc()
        {
            TDSTimer.OnUpdateFunc();
        }
    }
}
