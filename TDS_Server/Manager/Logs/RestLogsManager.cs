using GTANetworkAPI;
using System;
using System.Net;
using TDS_Common.Enum;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity.Log;

namespace TDS_Server.Manager.Logs
{
    internal static class RestLogsManager
    {
        public static void Log(ELogType type, Client source, bool saveipserial = false, bool savelobby = false)
        {
            bool ipAddressParseWorked = IPAddress.TryParse(source?.Address, out IPAddress address);
            var log = new LogRests
            {
                Type = type,
                Source = source?.GetEntity()?.Id ?? 0,
                Ip = saveipserial && ipAddressParseWorked ? address : null,
                Serial = saveipserial ? source?.Serial : null,
                Lobby = savelobby ? source?.GetChar().CurrentLobby?.Id : null,
                Timestamp = DateTime.UtcNow
            };
            LogsManager.AddLog(log);
        }
    }
}