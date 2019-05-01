using System.Collections.Generic;
using TDS_Client.Instance.Draw.Dx;
using TDS_Common.Instance.Utility;
using static RAGE.Events;

namespace TDS_Client.Manager.Utility
{
    internal class TimerManager : Script
    {
        public static ulong ElapsedTicks;

        public TimerManager()
        {
            TDSTimer.Init(RAGE.Chat.Output, GetTick);
            Tick += OnUpdateFunc;
            ElapsedTicks = GetTick();

            new TDSTimer(Dx.RefreshResolution, 10000, 0);
        }

        public static void OnUpdateFunc(List<TickNametagData> _)
        {
            ElapsedTicks = GetTick();
            TDSTimer.OnUpdateFunc();
        }

        private static ulong GetTick() => (ulong)RAGE.Game.Misc.GetGameTimer();
    }
}