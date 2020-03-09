using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Core.Manager.Utility
{
    internal static class OfflineMessagesManager
    {

        public static async void AddOfflineMessage(Players target, Players source, string message)
        {
            Offlinemessages msg = new Offlinemessages()
            {
                TargetId = target.Id,
                SourceId = source.Id,
                Message = message
            };
            using var dbContext = new TDSDbContext();
            dbContext.Add(msg);
            await dbContext.SaveChangesAsync();

            BonusBotConnector_Client.Requests.PrivateChat.SendOfflineMessage(source.GetDiscriminator(), message, target.PlayerSettings.DiscordUserId);
        }

        public static async void AddOfflineMessage(int targetId, ulong targetDiscordId, Players source, string message)
        {
            Offlinemessages msg = new Offlinemessages()
            {
                TargetId = targetId,
                SourceId = source.Id,
                Message = message
            };
            using var dbContext = new TDSDbContext();
            dbContext.Add(msg);
            await dbContext.SaveChangesAsync();

            BonusBotConnector_Client.Requests.PrivateChat.SendOfflineMessage(source.GetDiscriminator(), message, targetDiscordId);
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
                player.SendMessage(Utils.GetReplaced(player.Language.GOT_UNREAD_OFFLINE_MESSAGES, amountentries.ToString(), amountnewentries.ToString()));
            }
        }
    }
}