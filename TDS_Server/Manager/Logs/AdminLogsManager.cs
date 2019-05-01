using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;

namespace TDS_Server.Manager.Logs
{
    internal static class AdminLogsManager
    {
        private static readonly List<LogsAdmin> notsavedadminlogs = new List<LogsAdmin>();

        public static void Log(ELogType cmd, TDSPlayer? source, TDSPlayer? target, bool asdonator = false, bool asvip = false, string? reason = null)
        {
            notsavedadminlogs.Add(
                new LogsAdmin
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

        public static void Log(ELogType cmd, TDSPlayer? source, uint? targetid = null, bool asdonator = false, bool asvip = false, string? reason = null)
        {
            notsavedadminlogs.Add(
                new LogsAdmin
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
            if (notsavedadminlogs.Count == 0)
                return;
            await dbcontext.AddRangeAsync(notsavedadminlogs);
            await dbcontext.SaveChangesAsync();
            notsavedadminlogs.Clear();
        }
    }
}