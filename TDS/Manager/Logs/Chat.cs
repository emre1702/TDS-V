using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Instance.Player;
using TDS.Manager.Player;

namespace TDS.Manager.Logs
{
    class Chat
    {
        private static readonly List<LogsChat> notsavedchatlogs = new List<LogsChat>();

        public static void Log (string chat, Character source, Character target = null, bool isglobal = false, bool isadminchat = false, bool isteamchat = false)
        {
            notsavedchatlogs.Add(
                new LogsChat {
                    Source = source.Entity?.Id ?? 0,
                    Target = target?.Entity?.Id ?? null,
                    Message = chat,
                    Lobby = isglobal ? null : source?.CurrentLobby.Id,
                    IsAdminChat = isadminchat,
                    IsTeamChat = isteamchat
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
