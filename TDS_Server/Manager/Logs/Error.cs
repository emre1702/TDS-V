using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Logs
{
    class Error
    {
        private static readonly List<LogsError> notsavederrorlogs = new List<LogsError>();

        public static void Log(string info, string stacktrace, Client source)
        {
            notsavederrorlogs.Add(
                new LogsError
                {
                    Info = info,
                    StackTrace = stacktrace,
                    Source = source?.GetEntity()?.Id
                }
            );
        }

        public static async void Save(TDSNewContext dbcontext)
        {
            await dbcontext.AddRangeAsync(notsavederrorlogs);
            await dbcontext.SaveChangesAsync();
            notsavederrorlogs.Clear();
        }
    }
}
