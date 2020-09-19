using BonusBotConnector.Client;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Log;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler
{
    public class LoggingHandler : DatabaseEntityWrapper, ILoggingHandler
    {
        private static long _currentIdAdmins;
        private static long _currentIdChats;
        private static long _currentIdErrors;
        private static long _currentIdKills;
        private static long _currentIdRests;

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

            _currentIdAdmins = dbContext.LogAdmins.Max(a => a.Id);
            _currentIdChats = dbContext.LogChats.Max(a => a.Id);
            _currentIdErrors = dbContext.LogErrors.Max(a => a.Id);
            _currentIdKills = dbContext.LogKills.Max(a => a.Id);
            _currentIdRests = dbContext.LogRests.Max(a => a.Id);

            eventsHandler.Minute += Save;
            eventsHandler.Error += LogError;
            eventsHandler.ErrorMessage += LogError;
            eventsHandler.PlayerLeftLobbyNew += EventsHandler_PlayerLeftLobbyNew;

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

            NAPI.ClientEvent.Register<ITDSPlayer, string, string>(ToServerEvent.LogMessageToServer, this, LogMessageFromClient);
            NAPI.ClientEvent.Register<ITDSPlayer, string, string, string>(ToServerEvent.LogExceptionToServer, this, LogExceptionFromClient);
        }

        private async void LogExceptionFromClient(ITDSPlayer player, string message, string stackTrace, string typeName)
        {
            var log = new LogErrors
            {
                Id = ++_currentIdErrors,
                ExceptionType = typeName,
                Info = message,
                StackTrace = player.Name + " // " + player.SocialClubName + " // " + (stackTrace ?? Environment.StackTrace),
                Source = player.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine($"[{DateTime.Now}] {log.ExceptionType} {log.Info}{Environment.NewLine}{log.StackTrace}");

            await ExecuteForDB(dbContext =>
                dbContext.LogErrors.Add(log));

            _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
        }

        private async void LogMessageFromClient(ITDSPlayer player, string message, string source)
        {
            var log = new LogErrors
            {
                Id = ++_currentIdErrors,
                ExceptionType = "Message",
                Info = message,
                StackTrace = player.Name + " // " + player.SocialClubName + " // " + source,
                Source = player.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine($"[{DateTime.Now}] {log.ExceptionType} {log.Info}{Environment.NewLine}{log.StackTrace}");

            await ExecuteForDB(dbContext =>
                dbContext.LogErrors.Add(log));

            _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
        }

        private async void Save(int counter)
        {
            await SaveTask(counter);
        }

        public async Task SaveTask(int? counter = null)
        {
            if (counter is null || counter % _settingsHandler.ServerSettings.SaveLogsCooldownMinutes == 0)
                await ExecuteForDBAsync(async dbContext =>
                {
                    await dbContext.SaveChangesAsync();

                    var changedEntriesCopy = dbContext.ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Added ||
                                    e.State == EntityState.Modified ||
                                    e.State == EntityState.Deleted);
                    foreach (var entry in changedEntriesCopy)
                        entry.State = EntityState.Detached;
                });
        }

        #region Error

        public async void LogError(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true)
        {
            ex = ex.GetBaseException();
            var log = new LogErrors
            {
                Id = ++_currentIdErrors,
                ExceptionType = ex.GetType().Name,
                Info = ex.Message,
                StackTrace = ex.StackTrace ?? Environment.StackTrace,
                Source = source?.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine($"[{DateTime.Now}] {log.ExceptionType} {log.Info}{Environment.NewLine}{log.StackTrace}");

            await ExecuteForDB(dbContext =>
                dbContext.LogErrors.Add(log));

            if (logToBonusBot)
                _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
        }

        public async void LogError(string info, string? stackTrace = null, string? exceptionType = null, ITDSPlayer? source = null, bool logToBonusBot = true)
        {
            var log = new LogErrors
            {
                Id = ++_currentIdErrors,
                ExceptionType = exceptionType,
                Info = info,
                StackTrace = stackTrace ?? Environment.StackTrace,
                Source = source?.Id,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine($"[{DateTime.Now}] {log.ExceptionType} {log.Info}{Environment.NewLine}{log.StackTrace}");

            await ExecuteForDB(dbContext =>
                dbContext.LogErrors.Add(log));

            if (logToBonusBot)
                _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
        }

        public async void LogErrorFromBonusBot(Exception ex, bool logToBonusBot = true)
        {
            ex = ex.GetBaseException();
            var log = new LogErrors
            {
                Id = ++_currentIdErrors,
                ExceptionType = ex.GetType().Name,
                Info = ex.Message,
                StackTrace = ex.StackTrace ?? Environment.StackTrace,
                Source = -1,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine($"[{DateTime.Now}] {log.ExceptionType} {log.Info}{Environment.NewLine}{log.StackTrace}");

            await ExecuteForDB(dbContext =>
                dbContext.LogErrors.Add(log));

            if (logToBonusBot)
                _bonusBotConnectorClient.ChannelChat?.SendError(log.ToString());
        }

        public async void LogErrorFromBonusBot(string info, string stacktrace, string exceptionType, bool logToBonusBot = true)
        {
            var log = new LogErrors
            {
                Id = ++_currentIdErrors,
                ExceptionType = exceptionType,
                Info = info,
                StackTrace = stacktrace,
                Source = -1,
                Timestamp = DateTime.UtcNow
            };
            Console.WriteLine($"[{DateTime.Now}] " + info + "\n" + stacktrace);

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
                Id = ++_currentIdChats,
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
                Id = ++_currentIdAdmins,
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
                Id = ++_currentIdAdmins,
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
                Id = ++_currentIdKills,
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
            bool ipAddressParseWorked = IPAddress.TryParse(source?.IPAddress ?? "-", out IPAddress? address);
            var log = new LogRests
            {
                Id = ++_currentIdRests,
                Type = type,
                Source = source?.Id ?? 0,
                Ip = saveipserial && ipAddressParseWorked ? address : null,
                Serial = saveipserial ? source?.Serial ?? null : null,
                Lobby = savelobby ? source?.Lobby?.Id : null,
                Timestamp = DateTime.UtcNow
            };

            await ExecuteForDB(dbContext =>
                dbContext.LogRests.Add(log));
        }

        #endregion Rest

        private void EventsHandler_PlayerLeftLobbyNew(ITDSPlayer player, IBaseLobby lobby)
        {
            if (lobby.Type == LobbyType.MainMenu)
                return;

            if (lobby.IsRemoved)
                return;

            LoggingHandler?.LogRest(LogType.Lobby_Leave, player, false, lobby.IsOfficial);
        }
    }
}
