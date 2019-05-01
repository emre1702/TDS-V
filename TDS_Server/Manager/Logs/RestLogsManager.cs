using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Logs
{
    internal static class RestLogsManager
    {
        private static readonly List<LogsRest> notsavedrestlogs = new List<LogsRest>();

        public static void Log(ELogType type, Client source, bool saveipserial = false, bool savelobby = false)
        {
            notsavedrestlogs.Add(
                new LogsRest
                {
                    Type = (byte)type,
                    Source = source?.GetEntity()?.Id ?? 0,
                    Ip = saveipserial ? source?.Address : null,
                    Serial = saveipserial ? source?.Serial : null,
                    Lobby = savelobby ? source?.GetChar().CurrentLobby?.Id : null,
                    Timestamp = DateTime.Now
                }
            );
        }

        public static async Task Save(TDSNewContext dbcontext)
        {
            if (notsavedrestlogs.Count == 0)
                return;
            await dbcontext.AddRangeAsync(notsavedrestlogs);
            await dbcontext.SaveChangesAsync();
            notsavedrestlogs.Clear();
        }
    }
}