using GTANetworkAPI;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Manager.Utility
{
    class TimerManager : Script
    {
        public TimerManager()
        {
            Timer.Init(NAPI.Util.ConsoleOutput);
        }

        [ServerEvent(Event.Update)]
        public static void OnUpdateFunc()
        {
            Timer.OnUpdateFunc();
        }
    }
}
