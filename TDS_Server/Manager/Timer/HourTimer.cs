using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Userpanel;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Userpanel;

namespace TDS_Server.Manager.Timer
{
    static class HourTimer
    {
        private static readonly List<Func<Task>> _actions = new List<Func<Task>>
        {
            SupportRequest.DeleteTooLongClosedRequests,
            ApplicationUser.DeleteTooLongClosedApplications
        };

        public static async void Execute()
        {
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
    }
}
