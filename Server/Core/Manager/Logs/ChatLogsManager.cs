﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Log;

namespace TDS_Server.Core.Manager.Logs
{
    internal static class ChatLogsManager
    {
        public static void Log(string chat, TDSPlayer source, TDSPlayer? target = null, bool isglobal = false, bool isadminchat = false, bool isteamchat = false)
        {
            var log = new LogChats
            {
                Source = source.Entity?.Id ?? -1,
                Target = target?.Entity?.Id ?? null,
                Message = chat,
                Lobby = isglobal ? null : source?.CurrentLobby?.Id,
                IsAdminChat = isadminchat,
                IsTeamChat = isteamchat,
                Timestamp = DateTime.UtcNow
            };
            LogsManager.AddLog(log);
        }
    }
}