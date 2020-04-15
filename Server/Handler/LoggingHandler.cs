using BonusBotConnector.Client;
using System;
using System.Net;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Log;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler
{
    public class LoggingHandler : DatabaseEntityWrapper, ILoggingHandler
    {
        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly ISettingsHandler _settingsHandler;

        public LoggingHandler(TDSDbContext dbContext, BonusBotConnectorClient bonusBotConnectorClient, EventsHandler eventsHandler, 
            ISettingsHandler settingsHandler)
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            : base(dbContext, null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            _bonusBotConnectorClient = bonusBotConnectorClient;
            _settingsHandler = settingsHandler;
            LoggingHandler = this;

            eventsHandler.Minute += Save;
            eventsHandler.Error += LogError;

            if (_bonusBotConnectorClient.ChannelChat is { })
            {
                _bonusBotConnectorClient.ChannelChat.Error += LogErrorFromBonusBot;
                _bonusBotConnectorClient.ChannelChat.ErrorString += LogErrorFromBonusBot;
            }
            if (_bonusBotConnectorClient.PrivateChat is { })
            {
                _bonusBotConnectorClient.PrivateChat.Error += LogErrorFromBonusBot;
                _bonusBotConnectorClient.PrivateChat.ErrorString += LogErrorFromBonusBot;
            }
            if (_bonusBotConnectorClient.ServerInfos is { })
            {
                _bonusBotConnectorClient.ServerInfos.Error += LogErrorFromBonusBot;
                _bonusBotConnectorClient.ServerInfos.ErrorString += LogErrorFromBonusBot;
            }

        }

        private async void Save(int counter)
        {
            await SaveTask(counter);
        }

        public async Task SaveTask(int? counter = null)
        {
            if (counter is null || counter % _settingsHandler.ServerSettings.SaveLogsCooldownMinutes == 0)
                await ExecuteForDBAsync(async dbContext 
                    => await dbContext.SaveChangesAsync());
            
        }

        #region Error
        public async void LogError(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true)
        {
            var log = new LogErrors
            {
                Info = ex.GetBaseException().Message,
                StackTrace = ex.StackTrace ?? Environment.StackTrace,
                Source = source?.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine(log.Info + Environment.NewLine + log.StackTrace);

            await ExecuteForDB(dbContext => 
                dbContext.LogErrors.Add(log));

            if (logToBonusBot)
                _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
        }

        public async void LogError(string info, string? stackTrace = null, ITDSPlayer? source = null, bool logToBonusBot = true)
        {
            var log = new LogErrors
            {
                Info = info,
                StackTrace = stackTrace ?? Environment.StackTrace,
                Source = source?.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine(log.Info + Environment.NewLine + log.StackTrace);

            await ExecuteForDB(dbContext =>
                dbContext.LogErrors.Add(log));

            if (logToBonusBot)
                _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
        }

        public async void LogErrorFromBonusBot(Exception ex, bool logToBonusBot = true)
        {
            var log = new LogErrors
            {
                Info = ex.GetBaseException().Message,
                StackTrace = ex.StackTrace ?? Environment.StackTrace,
                Source = -1,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine(log.Info + Environment.NewLine + log.StackTrace);

            await ExecuteForDB(dbContext =>
                dbContext.LogErrors.Add(log));

            if (logToBonusBot)
                _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
        }

        public async void LogErrorFromBonusBot(string info, string stacktrace, bool logToBonusBot = true)
        {
            var log = new LogErrors
            {
                Info = info,
                StackTrace = stacktrace,
                Source = -1,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine(info + "\n" + stacktrace);

            await ExecuteForDB(dbContext =>
                dbContext.LogErrors.Add(log));


            if (logToBonusBot)
                _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
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
               _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
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
               _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
       }

      */
        #endregion Error

        #region Chat
        public async void LogChat(string chat, ITDSPlayer source, ITDSPlayer? target = null, bool isGlobal = false, bool isAdminChat = false, bool isTeamChat = false)
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

            await ExecuteForDB(dbContext =>
                dbContext.LogChats.Add(log));
        }
        #endregion Chat

        #region Admin
        public async void LogAdmin(LogType cmd, ITDSPlayer? source, ITDSPlayer? target, string reason, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null)
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

            await ExecuteForDB(dbContext =>
                dbContext.LogAdmins.Add(log));
        }

        public async void LogAdmin(LogType cmd, ITDSPlayer? source, string reason, int? targetid = null, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null)
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

            await ExecuteForDB(dbContext =>
                dbContext.LogAdmins.Add(log));
        }
        #endregion Admin

        #region Kill
        public async void LogKill(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            var log = new LogKills
            {
                KillerId = killer.Id,
                DeadId = player.Id,
                WeaponId = weapon
            };

            await ExecuteForDB(dbContext =>
                dbContext.LogKills.Add(log));
        }
        #endregion Kill

        #region Rest
        public async void LogRest(LogType type, ITDSPlayer source, bool saveipserial = false, bool savelobby = false)
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

            await ExecuteForDB(dbContext =>
                dbContext.LogRests.Add(log));
        }
        #endregion
    }
}
