using System;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;

namespace TDS_Server.Core.Manager.Timer
{
    public class TimerHandler
    {
        #region Private Fields

        private EventsHandler _eventsHandler;
        private TDSTimer _hourTimer;
        private TDSTimer _minuteTimer;
        private TDSTimer _secondTimer;

        #endregion Private Fields

        #region Public Constructors

        public TimerHandler(EventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;

            TDSTimer.Init(Console.WriteLine, () => Environment.TickCount);

            _hourTimer = new TDSTimer(OnHour, 60 * 1000, 1);
            _minuteTimer = new TDSTimer(OnMinute, Utils.GetMsToNextMinute(), 1);
            _secondTimer = new TDSTimer(OnSecond, Utils.GetMsToNextSecond(), 1);
        }

        #endregion Public Constructors

        #region Private Methods

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

        #endregion Private Methods
    }
}
