﻿using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Database.Entity.Player;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Interfaces
{
#nullable enable

    public interface ILoggingHandler
    {
        static ILoggingHandler? Instance { get; }

        void LogAdmin(LogType cmd, ITDSPlayer? source, ITDSPlayer? target, string reason, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null);

        void LogAdmin(LogType cmd, ITDSPlayer? source, string reason, int? targetid = null, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null);

        void LogChat(string chat, ITDSPlayer source, ITDSPlayer? target = null, bool isGlobal = false, bool isAdminChat = false, bool isTeamChat = false);

        void LogError(Exception ex, ITDSPlayer? source, bool logToBonusBot = true);

        void LogError(Exception ex, int? source = null, bool logToBonusBot = true);

        void LogError(string info, string? stackTrace = null, string? errorType = null, ITDSPlayer? source = null, bool logToBonusBot = true);

        void LogError(string info, string stackTrace, Players source, string? errorType = null, bool logToBonusBot = true);

        void LogErrorFromBonusBot(Exception ex, bool logToBonusBot = true);

        void LogErrorFromBonusBot(string info, string stacktrace, string exceptionType, bool logToBonusBot = true);

        void LogKill(ITDSPlayer player, ITDSPlayer killer, uint weapon);

        void LogRest(LogType type, ITDSPlayer source, bool saveipserial = false, bool savelobby = false, int? lobbyId = null);

        void AddErrorFile(string fileName, string fileContent);

        Task SaveTask(int? counter = null);
    }
}