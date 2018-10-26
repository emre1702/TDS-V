using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Manager.Player;

namespace TDS.Manager.Logs
{
    class Chat
    {
        private static readonly List<LogsChat> notsavedchatlogs = new List<LogsChat>();

        public static void Log (string chat, Client source, Client target, bool global, bool dirty)
        {
            // Don't log dirty or lobby chats
            if (dirty)
                return;
            //if (!global && !source?.GetLobby()?.IsOfficial)
                //return;

            notsavedchatlogs.Add(
                new LogsChat {
                    Source = source?.GetEntity()?.Id ?? 0,
                    Target = target?.GetEntity()?.Id ?? null,
                    Message = chat,
                    //Lobby = global ? null : source?.GetLobby()
                }
            );
        }

        public static async void Save(TDSNewContext dbcontext)
        {
            await dbcontext.AddRangeAsync(notsavedchatlogs);
            await dbcontext.SaveChangesAsync();
            notsavedchatlogs.Clear();
        }
    }
}
