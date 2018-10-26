using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Enum;
using TDS.Manager.Player;

namespace TDS.Manager.Logs
{
    class Rest
    {
        private static readonly List<LogsRest> notsavedrestlogs = new List<LogsRest>();

        public static void Log(ELogType type, Client source, bool saveipserial)
        {
            notsavedrestlogs.Add(
                new LogsRest
                {
                    Type = (byte)type,
                    Source = source?.GetEntity()?.Id ?? 0,
                    Ip = saveipserial ? source?.Address : null,
                    Serial = saveipserial ? source?.Serial : null,
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
