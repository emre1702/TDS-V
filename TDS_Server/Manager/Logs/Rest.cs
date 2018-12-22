using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Logs
{
    class Rest
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
                    Lobby = savelobby ? source?.GetChar().CurrentLobby?.Id : null
                }
            );
        }

        public static async void Save(TDSNewContext dbcontext)
        {
            await dbcontext.AddRangeAsync(notsavedrestlogs);
            await dbcontext.SaveChangesAsync();
            notsavedrestlogs.Clear();
        }
    }
}
