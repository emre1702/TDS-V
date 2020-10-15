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
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
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
#nullable disable
        public static ILoggingHandler Instance { get; private set; }
#nullable enable

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
            : base(dbContext)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            Instance = this;
            _bonusBotConnectorClient = bonusBotConnectorClient;
            _settingsHandler = settingsHandler;

            _currentIdAdmins = dbContext.LogAdmins.Any() ? dbContext.LogAdmins.Max(a => a.Id) : 0;
            _currentIdChats = dbContext.LogChats.Any() ? dbContext.LogChats.Max(a => a.Id) : 0;
            _currentIdErrors = dbContext.LogErrors.Any() ? dbContext.LogErrors.Max(a => a.Id) : 0;
            _currentIdKills = dbContext.LogKills.Any() ? dbContext.LogKills.Max(a => a.Id) : 0;
            _currentIdRests = dbContext.LogRests.Any() ? dbContext.LogRests.Max(a => a.Id) : 0;

            eventsHandler.Minute += Save;
            eventsHandler.Error += LogError;
            eventsHandler.ErrorMessage += LogError;
            eventsHandler.PlayerLeftLobby += EventsHandler_PlayerLeftLobby;
            eventsHandler.PlayerJoinedLobby += EventsHandler_PlayerJoinedLobby;

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
            try
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
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private async void LogMessageFromClient(ITDSPlayer player, string message, string source)
        {
            try
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
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private async void Save(int counter)
        {
            try
            {
                await SaveTask(counter);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
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
            try
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
            catch (Exception newEx)
            {
                Console.WriteLine($"[{DateTime.Now}] {ex.GetType().Name} {ex.Message}{Environment.NewLine}{ex.StackTrace ?? Environment.StackTrace}");
                Console.WriteLine($"[{DateTime.Now}] {newEx.GetType().Name} {newEx.Message}{Environment.NewLine}{newEx.StackTrace ?? Environment.StackTrace}");
            }
        }

        public async void LogError(string info, string? stackTrace = null, string? exceptionType = null, ITDSPlayer? source = null, bool logToBonusBot = true)
        {
            try
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
            catch (Exception newEx)
            {
                Console.WriteLine($"[{DateTime.Now}] {newEx.GetType().Name} {newEx.Message}{Environment.NewLine}{newEx.StackTrace ?? Environment.StackTrace}");
            }
        }

        public async void LogErrorFromBonusBot(Exception ex, bool logToBonusBot = true)
        {
            try
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
            catch (Exception newEx)
            {
                LogError(newEx);
            }
        }

        public async void LogErrorFromBonusBot(string info, string stacktrace, string exceptionType, bool logToBonusBot = true)
        {
            try
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
            catch (Exception newEx)
            {
                LogError(newEx);
            }
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
            try
            {
                var log = new LogChats
                {
                    Id = ++_currentIdChats,
                    Source = source.Entity?.Id ?? -1,
                    Target = target?.Entity?.Id ?? null,
                    Message = chat,
                    Lobby = isGlobal ? null : source?.Lobby?.Entity.Id,
                    IsAdminChat = isAdminChat,
                    IsTeamChat = isTeamChat,
                    Timestamp = DateTime.UtcNow
                };

                await ExecuteForDB(dbContext =>
                    dbContext.LogChats.Add(log));
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #endregion Chat

        #region Admin

        public async void LogAdmin(LogType cmd, ITDSPlayer? source, ITDSPlayer? target, string reason, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null)
        {
            try
            {
                var log = new LogAdmins
                {
                    Id = ++_currentIdAdmins,
                    Source = source?.Entity?.Id ?? -1,
                    Target = target?.Entity?.Id ?? null,
                    Type = cmd,
                    Lobby = target?.Lobby?.Entity.Id ?? source?.Lobby?.Entity.Id,
                    AsDonator = asdonator,
                    AsVip = asvip,
                    Reason = reason,
                    Timestamp = DateTime.UtcNow,
                    LengthOrEndTime = lengthOrEndTime
                };

                await ExecuteForDB(dbContext =>
                    dbContext.LogAdmins.Add(log));
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public async void LogAdmin(LogType cmd, ITDSPlayer? source, string reason, int? targetid = null, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null)
        {
            try
            {
                var log = new LogAdmins
                {
                    Id = ++_currentIdAdmins,
                    Source = source?.Entity?.Id ?? -1,
                    Target = targetid,
                    Type = cmd,
                    Lobby = source?.Lobby?.Entity.Id,
                    AsDonator = asdonator,
                    AsVip = asvip,
                    Reason = reason,
                    Timestamp = DateTime.UtcNow,
                    LengthOrEndTime = lengthOrEndTime
                };

                await ExecuteForDB(dbContext =>
                    dbContext.LogAdmins.Add(log));
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #endregion Admin

        #region Kill

        public async void LogKill(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            try
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
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #endregion Kill

        #region Rest

        public async void LogRest(LogType type, ITDSPlayer source, bool saveipserial = false, bool savelobby = false, int? lobbyId = null)
        {
            try
            {
                bool ipAddressParseWorked = IPAddress.TryParse(source?.Address ?? "-", out IPAddress? address);
                var log = new LogRests
                {
                    Id = ++_currentIdRests,
                    Type = type,
                    Source = source?.Id ?? 0,
                    Ip = saveipserial && ipAddressParseWorked ? address : null,
                    Serial = saveipserial ? source?.Serial ?? null : null,
                    Lobby = savelobby ? (lobbyId ?? source?.Lobby?.Entity.Id) : null,
                    Timestamp = DateTime.UtcNow
                };

                await ExecuteForDB(dbContext =>
                    dbContext.LogRests.Add(log));
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #endregion Rest

        private void EventsHandler_PlayerLeftLobby(ITDSPlayer player, IBaseLobby lobby)
        {
            if (lobby.Type == LobbyType.MainMenu)
                return;

            if (lobby.IsRemoved)
                return;

            LogRest(LogType.Lobby_Leave, player, false, lobby.IsOfficial, lobbyId: lobby.Entity.Id);
        }

        private void EventsHandler_PlayerJoinedLobby(ITDSPlayer player, IBaseLobby lobby)
        {
            if (lobby.Type == LobbyType.MainMenu)
                return;

            LogRest(LogType.Lobby_Join, player, false, lobby.IsOfficial, lobbyId: lobby.Entity.Id);
        }
    }
}
