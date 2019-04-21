using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Logs
{
    static class ErrorLogsManager
    {
        private static readonly List<LogsError> notsavederrorlogs = new List<LogsError>();

        public static void Log(string info, string stacktrace, Client source)
        {
            notsavederrorlogs.Add(
                new LogsError
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
            notsavederrorlogs.Add(
                new LogsError
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
            if (notsavederrorlogs.Count == 0)
                return;
            await dbcontext.AddRangeAsync(notsavederrorlogs);
            await dbcontext.SaveChangesAsync();
            notsavederrorlogs.Clear();
        }
    }
}
