using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Logs
{
    internal static class AdminLogsManager
    {
        public static void Log(ELogType cmd, TDSPlayer? source, TDSPlayer? target, string reason, bool asdonator = false, bool asvip = false)
        {
            var log = new LogAdmins
            {
                Source = source?.Entity?.Id ?? 0,
                Target = target?.Entity?.Id ?? null,
                Type = (byte)cmd,
                Lobby = target?.CurrentLobby?.Id ?? source?.CurrentLobby?.Id,
                AsDonator = asdonator,
                AsVip = asvip,
                Reason = reason,
                Timestamp = DateTime.Now
            };
            LogsManager.DbContext.Add(log);
        }

        public static void Log(ELogType cmd, TDSPlayer? source, string reason, int? targetid = null, bool asdonator = false, bool asvip = false)
        {
            var log = new LogAdmins
            {
                Source = source?.Entity?.Id ?? 0,
                Target = targetid,
                Type = (byte)cmd,
                Lobby = source?.CurrentLobby?.Id,
                AsDonator = asdonator,
                AsVip = asvip,
                Reason = reason,
                Timestamp = DateTime.Now
            };
            LogsManager.DbContext.Add(log);
        }
    }
}