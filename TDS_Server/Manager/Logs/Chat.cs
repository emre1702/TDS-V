using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Logs
{
    class Chat
    {
        private static readonly List<LogsChat> notsavedchatlogs = new List<LogsChat>();

        public static void Log (string chat, TDSPlayer source, TDSPlayer target = null, bool isglobal = false, bool isadminchat = false, bool isteamchat = false)
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
