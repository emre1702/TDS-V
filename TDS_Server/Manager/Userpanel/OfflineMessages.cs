using GTANetworkAPI;
using MessagePack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Manager.Userpanel
{
    class OfflineMessages
    {
        public static async Task<string?> GetData(TDSPlayer player)
        {
            using var dbContext = new TDSDbContext();

            var offlineMessages = await dbContext.Offlinemessages
                .Where(o => o.TargetId == player.Entity!.Id)
                .Include(o => o.Source)
                .Select(o => new OfflineMessage
                {
                    ID = o.Id,
                    PlayerName = o.Source.Name,
                    CreateTimeDate = o.Timestamp,
                    Text = o.Message,
                    Seen = o.Seen
                })
                .ToListAsync();

            foreach (var message in offlineMessages)
            {
                message.CreateTime = player.GetLocalDateTimeString(message.CreateTimeDate);
            }

            string json = Serializer.ToBrowser(offlineMessages);

            if (offlineMessages.Any(m => !m.Seen))
            {
                foreach (var message in offlineMessages.Where(m => !m.Seen))
                {
                    message.Seen = true;
                }
                await dbContext.SaveChangesAsync();
            }

            return json;
        }

        public static async Task<object?> Answer(TDSPlayer player, params object[] args)
        {
            int? offlineMessageID;
            if ((offlineMessageID = Utils.GetInt(args[0])) is null)
                return null;
            string? message = Convert.ToString(args[1]);
            if (message is null)
                return null;

            using var dbContext = new TDSDbContext();

            var offlineMessage = await dbContext.Offlinemessages
                .Include(o => o.Source)
                .ThenInclude(s => s.PlayerSettings)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == offlineMessageID);
            if (offlineMessage is null)
                return null;

            OfflineMessagesManager.AddOfflineMessage(offlineMessage.Source, player.Entity!, message);

            return null;
        }

        public static async Task<object?> Send(TDSPlayer player, params object[] args)
        {
            string? playerName = Convert.ToString(args[0]);
            if (playerName is null)
                return false;
            int? targetId;
            if (!(targetId = Utils.GetInt(args[1])).HasValue)
            {
                targetId = await Player.Player.GetPlayerIDByName(playerName);
                if (targetId == 0)
                {
                    player.SendNotification(player.Language.PLAYER_DOESNT_EXIST, true);
                    return false;
                }
            }

            string? message = Convert.ToString(args[1]);
            if (message is null)
                return false;

            using var dbContext = new TDSDbContext();

            var discordUserId = await dbContext.PlayerSettings
                .Where(p => p.PlayerId == targetId.Value)
                .Select(p => p.DiscordUserId)
                .FirstOrDefaultAsync();

            OfflineMessagesManager.AddOfflineMessage(targetId.Value, discordUserId, player.Entity!, message);

            return true;
        }

        public static async Task<object?> Delete(TDSPlayer player, params object[] args)
        {
            int? offlineMessageId;
            if ((offlineMessageId = Utils.GetInt(args[0])) is null)
                return null;

            using var dbContext = new TDSDbContext();

            var offlineMessage = await dbContext.Offlinemessages.FirstOrDefaultAsync(o => o.Id == offlineMessageId);
            if (offlineMessage is null)
                return null;

            dbContext.Offlinemessages.Remove(offlineMessage);

            await dbContext.SaveChangesAsync();
            return null;
        }

        public static async Task DeleteOldMessages()
        {
            using var dbContext = new TDSDbContext();

            var deleteAfterDays = SettingsManager.ServerSettings.DeleteOfflineMessagesAfterDays;
            await dbContext.Offlinemessages.Where(o => o.Timestamp.AddDays(deleteAfterDays) < DateTime.UtcNow).DeleteFromQueryAsync();
        }
    }

    [MessagePackObject]
    public class OfflineMessage
    {
        [Key(0)]
        public int ID { get; set; }
        [Key(1)]
        public string PlayerName { get; set; } = string.Empty;
        [Key(2)]
        public string CreateTime { get; set; } = string.Empty;
        [Key(3)]
        public string Text { get; set; } = string.Empty;
        [Key(4)]
        public bool Seen { get; set; }

        [IgnoreMember]
        public DateTime CreateTimeDate { get; set; }
    }
}
