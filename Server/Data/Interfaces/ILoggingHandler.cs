using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces
{
    #nullable enable
    public interface ILoggingHandler
    {
        void LogError(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true);
        void LogError(string info, string? stackTrace = null, ITDSPlayer? source = null, bool logToBonusBot = true);
        void LogErrorFromBonusBot(Exception ex, bool logToBonusBot = true);
        void LogErrorFromBonusBot(string info, string stacktrace, bool logToBonusBot = true);
        void LogChat(string chat, ITDSPlayer source, ITDSPlayer? target = null, bool isGlobal = false, bool isAdminChat = false, bool isTeamChat = false);
        void LogAdmin(LogType cmd, ITDSPlayer? source, ITDSPlayer? target, string reason, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null);
        void LogAdmin(LogType cmd, ITDSPlayer? source, string reason, int? targetid = null, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null);
        void LogKill(ITDSPlayer player, ITDSPlayer killer, uint weapon);
        void LogRest(LogType type, ITDSPlayer source, bool saveipserial = false, bool savelobby = false);
        Task SaveTask(int? counter = null);
    }
}
