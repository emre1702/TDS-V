using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Enum;
using TDS.Manager.Player;

namespace TDS.Manager.Logs
{
    class Admin
    {
        private static readonly List<LogsAdmin> notsavedadminlogs = new List<LogsAdmin>();

        public static void Log (ELogType cmd, Client source, Client target, string info, bool asdonator, bool asvip)
        {

            notsavedadminlogs.Add(
                new LogsAdmin
                {
                    Source = source?.GetEntity()?.Id ?? 0,
                    Target = target?.GetEntity()?.Id ?? null,
                    Type = (byte)cmd,
                    AsDonator = asdonator,
                    AsVip = asvip,
                    //Lobby = source?.GetLobby()?.Id,
                    //Lobby = global ? null : source?.GetLobby()
                }
            );
        }

        public static async void Save(TDSNewContext dbcontext)
        {
            await dbcontext.AddRangeAsync(notsavedadminlogs);
            await dbcontext.SaveChangesAsync();
            notsavedadminlogs.Clear();
        }
    }
}