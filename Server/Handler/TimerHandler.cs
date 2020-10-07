using System;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;

namespace TDS_Server.Handler
{
    public class TimerHandler
    {
        private readonly EventsHandler _eventsHandler;

        public TimerHandler(EventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;

            TDSTimer.Init(Console.WriteLine, () => Environment.TickCount);

            _eventsHandler.Update += TDSTimer.OnUpdateFunc;

            new TDSTimer(OnHour, 60 * 1000, 1);
            new TDSTimer(OnMinute, Utils.GetMsToNextMinute(), 1);
            new TDSTimer(OnSecond, Utils.GetMsToNextSecond(), 1);
        }

        private void OnHour()
        {
            new TDSTimer(OnHour, Utils.GetMsToNextHour(), 1);
            _eventsHandler.OnHour();
        }

        private void OnMinute()
        {
            new TDSTimer(OnMinute, Utils.GetMsToNextMinute(), 1);
            _eventsHandler.OnMinute();
        }

        private void OnSecond()
        {
            new TDSTimer(OnSecond, Utils.GetMsToNextSecond(), 1);
            _eventsHandler.OnSecond();
        }
    }
}
