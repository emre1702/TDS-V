using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Manager.Player;

namespace TDS.Manager.Logs
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
