using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Logs
{
    internal static class ChatLogsManager
    {
        private static readonly List<LogChats> _notSavedChatLogs = new List<LogChats>();

        public static void Log(string chat, TDSPlayer source, TDSPlayer? target = null, bool isglobal = false, bool isadminchat = false, bool isteamchat = false)
        {
            _notSavedChatLogs.Add(
                new LogChats
                {
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
            if (_notSavedChatLogs.Count == 0)
                return;
            await dbcontext.AddRangeAsync(_notSavedChatLogs);
            await dbcontext.SaveChangesAsync();
            _notSavedChatLogs.Clear();
        }
    }
}