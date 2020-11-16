using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Utility;
using TDS.Server.Handler.Events;
using TDS.Shared.Core;

namespace TDS.Server.Handler
{
    public class TimerHandler
    {
        private readonly EventsHandler _eventsHandler;

        public TimerHandler(ILoggingHandler loggingHandler, EventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;

            TDSTimer.Init(ex => loggingHandler.LogError(ex), () => Environment.TickCount);

            _eventsHandler.Update += TDSTimer.OnUpdateFunc;

            _ = new TDSTimer(OnHour, 60 * 1000, 1);
            _ = new TDSTimer(OnMinute, Utils.GetMsToNextMinute(), 1);
            _ = new TDSTimer(OnSecond, Utils.GetMsToNextSecond(), 1);
        }

        private void OnHour()
        {
            _ = new TDSTimer(OnHour, Utils.GetMsToNextHour(), 1);
            _eventsHandler.OnHour();
        }

        private void OnMinute()
        {
            _ = new TDSTimer(OnMinute, Utils.GetMsToNextMinute(), 1);
            _eventsHandler.OnMinute();
        }

        private void OnSecond()
        {
            _ = new TDSTimer(OnSecond, Utils.GetMsToNextSecond(), 1);
            _eventsHandler.OnSecond();
        }
    }
}
