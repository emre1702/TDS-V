using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TDS_Common.Enum;
using TDS_Server.Enum;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity;

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
                Ip = ipAddressParseWorked ? address : null,
                Serial = saveipserial ? source?.Serial : null,
                Lobby = savelobby ? source?.GetChar().CurrentLobby?.Id : null,
                Timestamp = DateTime.Now
            };
            LogsManager.DbContext.Add(log);
        }
    }
}