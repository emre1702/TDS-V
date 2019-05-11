using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Logs
{
    internal static class ErrorLogsManager
    {
        private static readonly List<LogErrors> _notSavedErrorLogs = new List<LogErrors>();

        public static void Log(string info, string stacktrace, Client source)
        {
            _notSavedErrorLogs.Add(
                new LogErrors
                {
                    Info = info,
                    StackTrace = stacktrace,
                    Source = source?.GetEntity()?.Id,
                    Timestamp = DateTime.Now
                }
            );
            Console.WriteLine(info + "\n" + stacktrace);
        }

        public static void Log(string info, string stacktrace, TDSPlayer? source = null)
        {
            _notSavedErrorLogs.Add(
                new LogErrors
                {
                    Info = info,
                    StackTrace = stacktrace,
                    Source = source?.Entity?.Id,
                    Timestamp = DateTime.Now
                }
            );
            Console.WriteLine(info + "\n" + stacktrace);
        }

        public static async Task Save(TDSNewContext dbcontext)
        {
            if (_notSavedErrorLogs.Count == 0)
                return;
            await dbcontext.AddRangeAsync(_notSavedErrorLogs);
            await dbcontext.SaveChangesAsync();
            _notSavedErrorLogs.Clear();
        }
    }
}