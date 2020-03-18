using System;
using System.Net;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Log;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler
{
    public class LoggingHandler
    {
        private TDSDbContext _dbContext;

        public LoggingHandler(TDSDbContext dbContext)
            => _dbContext = dbContext;

        #region Error
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
                BonusBotConnector.Client.Requests.ChannelChat.SendError(log.ToString());
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
                BonusBotConnector.Client.Requests.ChannelChat.SendError(log.ToString());
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
        #endregion Error

        #region Chat
        public void LogChat(string chat, ITDSPlayer source, ITDSPlayer? target = null, bool isGlobal = false, bool isAdminChat = false, bool isTeamChat = false)
        {
            var log = new LogChats
            {
                Source = source.Entity?.Id ?? -1,
                Target = target?.Entity?.Id ?? null,
                Message = chat,
                Lobby = isGlobal ? null : source?.Lobby?.Id,
                IsAdminChat = isAdminChat,
                IsTeamChat = isTeamChat,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.LogChats.Add(log);
        }
        #endregion Chat

        #region Admin
        public void LogAdmin(LogType cmd, TDSPlayer? source, TDSPlayer? target, string reason, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null)
        {
            var log = new LogAdmins
            {
                Source = source?.Entity?.Id ?? -1,
                Target = target?.Entity?.Id ?? null,
                Type = cmd,
                Lobby = target?.Lobby?.Id ?? source?.Lobby?.Id,
                AsDonator = asdonator,
                AsVip = asvip,
                Reason = reason,
                Timestamp = DateTime.UtcNow,
                LengthOrEndTime = lengthOrEndTime
            };
            _dbContext.LogAdmins.Add(log);
        }

        public void LogAdmin(LogType cmd, TDSPlayer? source, string reason, int? targetid = null, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null)
        {
            var log = new LogAdmins
            {
                Source = source?.Entity?.Id ?? -1,
                Target = targetid,
                Type = cmd,
                Lobby = source?.Lobby?.Id,
                AsDonator = asdonator,
                AsVip = asvip,
                Reason = reason,
                Timestamp = DateTime.UtcNow,
                LengthOrEndTime = lengthOrEndTime
            };
            _dbContext.LogAdmins.Add(log);
        }
        #endregion Admin

        #region Kill
        public void LogKill(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            var log = new LogKills
            {
                KillerId = killer.Id,
                DeadId = player.Id,
                WeaponId = weapon
            };
            _dbContext.LogKills.Add(log);
        }
        #endregion Kill

        #region Rest
        public void LogRest(LogType type, ITDSPlayer source, bool saveipserial = false, bool savelobby = false)
        {
            bool ipAddressParseWorked = IPAddress.TryParse(source?.ModPlayer?.IPAddress ?? "-", out IPAddress address);
            var log = new LogRests
            {
                Type = type,
                Source = source?.Id ?? 0,
                Ip = saveipserial && ipAddressParseWorked ? address : null,
                Serial = saveipserial ? source?.ModPlayer?.Serial ?? null : null,
                Lobby = savelobby ? source?.Lobby?.Id : null,
                Timestamp = DateTime.UtcNow
            };
            _dbContext.LogRests.Add(log);
        }
        #endregion
    }
}
