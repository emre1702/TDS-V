
using System.Collections.Generic;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Player;

namespace TDS.Manager.Logs
{
    class Admin
    {
        private static readonly List<LogsAdmin> notsavedadminlogs = new List<LogsAdmin>();

        public static void Log (ELogType cmd, Character source, string info, Character target = null, bool asdonator = false, bool asvip = false)
        {

            notsavedadminlogs.Add(
                new LogsAdmin
                {
                    Source = source?.Entity?.Id ?? 0,
                    Target = target?.Entity?.Id ?? null,
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