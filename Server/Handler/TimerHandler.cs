using System;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Events;
using TDS_Shared.Instance;

namespace TDS_Server.Core.Manager.Timer
{
    public class TimerHandler
    {
        private TDSTimer _hourTimer;
        private TDSTimer _minuteTimer;
        private TDSTimer _secondTimer;
        private EventsHandler _eventsHandler;

        public TimerHandler(EventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;

            TDSTimer.Init(Console.WriteLine, () => (ulong)Environment.TickCount & int.MaxValue);

            _eventsHandler.Update += TDSTimer.OnUpdateFunc;

            _hourTimer = new TDSTimer(OnHour, 60 * 1000, 1);   
            _minuteTimer = new TDSTimer(OnMinute, Utils.GetMsToNextMinute(), 1);
            _secondTimer = new TDSTimer(OnSecond, Utils.GetMsToNextSecond(), 1);

        }

        private void OnHour()
        {
            _hourTimer = new TDSTimer(OnHour, Utils.GetMsToNextHour(), 1);
            _eventsHandler.OnHour();
        }

        private void OnMinute()
        {
            _hourTimer = new TDSTimer(OnMinute, Utils.GetMsToNextMinute(), 1);
            _eventsHandler.OnMinute();
        }

        private void OnSecond()
        {
            _hourTimer = new TDSTimer(OnSecond, Utils.GetMsToNextSecond(), 1);
            _eventsHandler.OnSecond();
        }
    }
}
