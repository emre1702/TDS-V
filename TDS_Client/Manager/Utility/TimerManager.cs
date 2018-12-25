using System.Collections.Generic;
using TDS_Common.Instance.Utility;
using static RAGE.Events;

namespace TDS_Client.Manager.Utility
{
    class TimerManager : Script
    {

        public TimerManager()
        {
            TDSTimer.Init(RAGE.Chat.Output);
            Tick += OnUpdateFunc;
        }

        public static void OnUpdateFunc(List<TickNametagData> _)
        {
            TDSTimer.OnUpdateFunc();
        }
    }
}
