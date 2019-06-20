using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Logs
{
    internal static class ChatLogsManager
    {
        public static void Log(string chat, TDSPlayer source, TDSPlayer? target = null, bool isglobal = false, bool isadminchat = false, bool isteamchat = false)
        {
            var log = new LogChats
            {
                Source = source.Entity?.Id ?? 0,
                Target = target?.Entity?.Id ?? null,
                Message = chat,
                Lobby = isglobal ? null : source?.CurrentLobby?.Id,
                IsAdminChat = isadminchat,
                IsTeamChat = isteamchat,
                Timestamp = DateTime.Now
            };
            LogsManager.DbContext.Add(log);
        }
    }
}