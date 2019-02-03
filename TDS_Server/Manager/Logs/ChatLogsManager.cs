using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;

namespace TDS_Server.Manager.Logs
{
    static class ChatLogsManager
    {
        private static readonly List<LogsChat> notsavedchatlogs = new List<LogsChat>();

        public static void Log (string chat, TDSPlayer source, TDSPlayer target = null, bool isglobal = false, bool isadminchat = false, bool isteamchat = false)
        {
            notsavedchatlogs.Add(
                new LogsChat {
                    Source = source.Entity?.Id ?? 0,
                    Target = target?.Entity?.Id ?? null,
                    Message = chat,
                    Lobby = isglobal ? null : source?.CurrentLobby?.Id,
                    IsAdminChat = isadminchat,
                    IsTeamChat = isteamchat,
                    Timestamp = DateTime.Now
                }
            );
        }

        public static async Task Save(TDSNewContext dbcontext)
        {
            if (notsavedchatlogs.Count == 0)
                return;
            await dbcontext.AddRangeAsync(notsavedchatlogs);
            await dbcontext.SaveChangesAsync();
            notsavedchatlogs.Clear();
        }
    }
}
