using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Log;

namespace TDS_Server.Handler
{
    public class LoggingHandler
    {
        private TDSDbContext _dbContext;

        public LoggingHandler(TDSDbContext dbContext)
            => _dbContext = dbContext;

        public void LogError(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true)
        {
            var log = new LogErrors
            {
                Info = ex.GetBaseException().Message,
                StackTrace = ex.StackTrace ?? Environment.StackTrace,
                Source = source?.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine(log.Info + Environment.StackTrace + log.StackTrace);

            _dbContext.LogErrors.Add(log);

            if (logToBonusBot)
                BonusBotConnector_Client.Requests.ChannelChat.SendError(log.ToString());
        }

        public void LogError(string info, string? stackTrace = null, ITDSPlayer? source = null, bool logToBonusBot = true)
        {
            var log = new LogErrors
            {
                Info = info,
                StackTrace = stackTrace ?? Environment.StackTrace,
                Source = source?.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine(log.Info + Environment.StackTrace + log.StackTrace);

            _dbContext.LogErrors.Add(log);

            if (logToBonusBot)
                BonusBotConnector_Client.Requests.ChannelChat.SendError(log.ToString());
        }

        /*public static void LogError(string info, string stacktrace, Player source, bool logToBonusBot = true)
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
            if (logToBonusBot)
                BonusBotConnector_Client.Requests.ChannelChat.SendError(log.ToString());
        }

        public void LogError(string info, string stacktrace, TDSPlayer? source = null, bool logToBonusBot = true)
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
            if (logToBonusBot)
                BonusBotConnector_Client.Requests.ChannelChat.SendError(log.ToString());
        }

        public void LogErrorFromBonusBot(string info, string stacktrace, bool logToBonusBot = true)
        {
            var log = new LogErrors
            {
                Info = info,
                StackTrace = stacktrace,
                Source = -1,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine(info + "\n" + stacktrace);
            LogsManager.AddLog(log);
            if (logToBonusBot)
                BonusBotConnector_Client.Requests.ChannelChat.SendError(log.ToString());
        }*/
    }
}
