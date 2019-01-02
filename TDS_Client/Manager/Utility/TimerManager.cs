using System.Collections.Generic;
using TDS_Common.Instance.Utility;
using static RAGE.Events;

namespace TDS_Client.Manager.Utility
{
    class TimerManager : Script
    {
        public static ulong ElapsedTicks;

        public TimerManager()
        {
            TDSTimer.Init(RAGE.Chat.Output, () => ElapsedTicks);
            Tick += OnUpdateFunc;
        }

        public static void OnUpdateFunc(List<TickNametagData> _)
        {
            ++ElapsedTicks;
            TDSTimer.OnUpdateFunc();
        }
    }
}
