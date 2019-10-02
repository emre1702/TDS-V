using System;
using TDS_Common.Enum;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Log;

namespace TDS_Server.Manager.Logs
{
    internal static class AdminLogsManager
    {
        public static void Log(ELogType cmd, TDSPlayer? source, TDSPlayer? target, string reason, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null)
        {
            var log = new LogAdmins
            {
                Source = source?.Entity?.Id ?? 0,
                Target = target?.Entity?.Id ?? null,
                Type = cmd,
                Lobby = target?.CurrentLobby?.Id ?? source?.CurrentLobby?.Id,
                AsDonator = asdonator,
                AsVip = asvip,
                Reason = reason,
                Timestamp = DateTime.Now,
                LengthOrEndTime = lengthOrEndTime
            };
            LogsManager.AddLog(log);
        }

        public static void Log(ELogType cmd, TDSPlayer? source, string reason, int? targetid = null, bool asdonator = false, bool asvip = false, string? lengthOrEndTime = null)
        {
            var log = new LogAdmins
            {
                Source = source?.Entity?.Id ?? 0,
                Target = targetid,
                Type = cmd,
                Lobby = source?.CurrentLobby?.Id,
                AsDonator = asdonator,
                AsVip = asvip,
                Reason = reason,
                Timestamp = DateTime.Now,
                LengthOrEndTime = lengthOrEndTime
            };
            LogsManager.AddLog(log);
        }
    }
}