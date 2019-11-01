using GTANetworkAPI;
using System;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity.Log;

namespace TDS_Server.Manager.Logs
{
    internal static class ErrorLogsManager
    {
        public static void Log(string info, string stacktrace, Client source)
        {
            var log = new LogErrors
            {
                Info = info,
                StackTrace = stacktrace,
                Source = source?.GetEntity()?.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine(info + "\n" + stacktrace);
            LogsManager.AddLog(log);
        }

        public static void Log(string info, string stacktrace, TDSPlayer? source = null)
        {
            var log = new LogErrors
            {
                Info = info,
                StackTrace = stacktrace,
                Source = source?.Entity?.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine(info + "\n" + stacktrace);
            LogsManager.AddLog(log);
        }
    }
}