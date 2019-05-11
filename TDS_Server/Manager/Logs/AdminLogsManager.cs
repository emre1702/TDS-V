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
        private static readonly List<LogAdmins> _notSavedAdminLogs = new List<LogAdmins>();

        public static void Log(ELogType cmd, TDSPlayer? source, TDSPlayer? target, bool asdonator = false, bool asvip = false, string? reason = null)
        {
            _notSavedAdminLogs.Add(
                new LogAdmins
                {
                    Source = source?.Entity?.Id ?? 0,
                    Target = target?.Entity?.Id ?? null,
                    Type = (byte)cmd,
                    Lobby = target?.CurrentLobby?.Id ?? source?.CurrentLobby?.Id,
                    AsDonator = asdonator,
                    AsVip = asvip,
                    Reason = reason,
                    Timestamp = DateTime.Now
                }
            );
        }

        public static void Log(ELogType cmd, TDSPlayer? source, int? targetid = null, bool asdonator = false, bool asvip = false, string? reason = null)
        {
            _notSavedAdminLogs.Add(
                new LogAdmins
                {
                    Source = source?.Entity?.Id ?? 0,
                    Target = targetid,
                    Type = (byte)cmd,
                    Lobby = source?.CurrentLobby?.Id,
                    AsDonator = asdonator,
                    AsVip = asvip,
                    Reason = reason,
                    Timestamp = DateTime.Now
                }
            );
        }

        public static async Task Save(TDSNewContext dbcontext)
        {
            if (_notSavedAdminLogs.Count == 0)
                return;
            await dbcontext.AddRangeAsync(_notSavedAdminLogs);
            await dbcontext.SaveChangesAsync();
            _notSavedAdminLogs.Clear();
        }
    }
}