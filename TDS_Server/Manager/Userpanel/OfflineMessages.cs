using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            using var dbContext = new TDSNewContext();

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

            string json = JsonConvert.SerializeObject(offlineMessages);

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

            using var dbContext = new TDSNewContext();

            var offlineMessage = await dbContext.Offlinemessages.AsNoTracking().FirstOrDefaultAsync(o => o.Id == offlineMessageID);
            if (offlineMessage is null)
                return null;

            var newOfflineMessage = new Offlinemessages
            {
                Seen = false,
                Message = message,
                SourceId = player.Entity!.Id,
                TargetId = offlineMessage.SourceId
            };
            dbContext.Offlinemessages.Add(newOfflineMessage);

            await dbContext.SaveChangesAsync();
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
                    NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.PLAYER_DOESNT_EXIST, true);
                    return false;
                }
            }

            string? message = Convert.ToString(args[1]);
            if (message is null)
                return false;

            using var dbContext = new TDSNewContext();

            var newOfflineMessage = new Offlinemessages
            {
                Seen = false,
                Message = message,
                SourceId = player.Entity!.Id,
                TargetId = targetId.Value
            };
            dbContext.Offlinemessages.Add(newOfflineMessage);

            await dbContext.SaveChangesAsync();
            return true;
        }

        private class OfflineMessage {
            public int ID { get; set; }
            public string PlayerName { get; set; } = string.Empty;
            public string CreateTime { get; set; } = string.Empty;
            public string Text { get; set; } = string.Empty;
            public bool Seen { get; set; }

            [JsonIgnore]
            public DateTime CreateTimeDate { get; set; }
        }
    }
}
