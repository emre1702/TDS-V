using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TDS_Server.Enum;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Logs
{
    internal static class RestLogsManager
    {
        private static readonly List<LogRests> _notSavedRestLogs = new List<LogRests>();

        public static void Log(ELogType type, Client source, bool saveipserial = false, bool savelobby = false)
        {
            bool ipAddressParseWorked = IPAddress.TryParse(source?.Address, out IPAddress address);
            _notSavedRestLogs.Add(
                new LogRests
                {
                    Type = (short)type,
                    Source = source?.GetEntity()?.Id ?? 0,
                    Ip = ipAddressParseWorked ? address : null,
                    Serial = saveipserial ? source?.Serial : null,
                    Lobby = savelobby ? source?.GetChar().CurrentLobby?.Id : null,
                    Timestamp = DateTime.Now
                }
            );
        }

        public static async Task Save(TDSNewContext dbcontext)
        {
            if (_notSavedRestLogs.Count == 0)
                return;
            await dbcontext.AddRangeAsync(_notSavedRestLogs);
            await dbcontext.SaveChangesAsync();
            _notSavedRestLogs.Clear();
        }
    }
}