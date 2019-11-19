using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Manager.Utility
{
    internal static class OfflineMessagesManager
    {

        public static async void AddOfflineMessage(int playerid, int sourceid, string message)
        {
            Offlinemessages msg = new Offlinemessages()
            {
                TargetId = playerid,
                SourceId = sourceid,
                Message = message
            };
            using var dbContext = new TDSDbContext();
            dbContext.Add(msg);
            await dbContext.SaveChangesAsync();
        }

        public static async void CheckOfflineMessages(TDSPlayer player)
        {
            using var dbContext = new TDSDbContext();
            int amountnewentries = await dbContext.Offlinemessages
                .Where(msg => player.Entity != null && msg.SourceId == player.Entity.Id && !msg.Seen)
                .AsNoTracking()
                .CountAsync();
            int amountentries = await dbContext.Offlinemessages
                .AsNoTracking()
                .CountAsync();

            if (amountnewentries > 0)
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client,
                    Utils.GetReplaced(player.Language.GOT_UNREAD_OFFLINE_MESSAGES, amountentries.ToString(), amountnewentries.ToString())
                );
            }
        }
    }
}