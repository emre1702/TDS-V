using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Common.Instance.Utility;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Userpanel;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Timer
{
    static class HourTimer
    {
        private static readonly List<Func<Task>> _actions = new List<Func<Task>>
        {
            SupportRequest.DeleteTooLongClosedRequests,
            ApplicationUser.DeleteTooLongClosedApplications,
            OfflineMessages.DeleteOldMessages,


            ResourceStop.CheckHourForResourceRestart
        };

        public static async void Execute()
        {
            CreateTimer();
            foreach (var action in _actions)
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    ErrorLogsManager.Log(ex.Message, Environment.StackTrace);
                }
            }
        }

        public static void CreateTimer()
        {
            _ = new TDSTimer(Execute, Utils.GetMsToNextHour(), 1);
        }
    }
}
