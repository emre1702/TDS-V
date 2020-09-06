using System.Collections.Generic;
using TDS_Shared.Core;
using static RAGE.Events;

namespace TDS_Client.Handler
{
    public class TimerHandler : ServiceBase
    {
        public int ElapsedMs;

        public TimerHandler(LoggingHandler loggingHandler)
            : base(loggingHandler)
        {
            ElapsedMs = RAGE.Game.Misc.GetGameTimer();

            TDSTimer.Init(RAGE.Chat.Output, () => ElapsedMs);

            Tick += (_) => TDSTimer.OnUpdateFunc();
            Tick += RefreshElapsedMs;
        }

        private void RefreshElapsedMs(List<TickNametagData> _)
        {
            ElapsedMs = RAGE.Game.Misc.GetGameTimer();
        }
    }
}
