using BonusBotConnector.Client;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;

namespace TDS_Server.Core.Manager.Utility
{
    public class OfflineMessagesHandler : DatabaseEntityWrapper
    {
        private readonly BonusBotConnectorClient _bonusBotConnectorClient;

        public OfflineMessagesHandler(EventsHandler eventsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler, BonusBotConnectorClient bonusBotConnectorClient) 
            : base(dbContext, loggingHandler)
        {
            _bonusBotConnectorClient = bonusBotConnectorClient;

            eventsHandler.PlayerLoggedIn += CheckOfflineMessages;
        }

        public async void AddOfflineMessage(Players target, Players source, string message)
        {
            Offlinemessages msg = new Offlinemessages()
            {
                TargetId = target.Id,
                SourceId = source.Id,
                Message = message
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Add(msg);
                await dbContext.SaveChangesAsync();
            });

            _bonusBotConnectorClient.PrivateChat?.SendOfflineMessage(source.GetDiscriminator(), message, target.PlayerSettings.DiscordUserId);
        }

        public async void Add(int targetId, ulong targetDiscordId, Players source, string message)
        {
            Offlinemessages msg = new Offlinemessages()
            {
                TargetId = targetId,
                SourceId = source.Id,
                Message = message
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Add(msg);
                await dbContext.SaveChangesAsync();
            });

            _bonusBotConnectorClient.PrivateChat?.SendOfflineMessage(source.GetDiscriminator(), message, targetDiscordId);
        }

        public async void CheckOfflineMessages(ITDSPlayer player)
        {
            (int amountNewEntries, int amountEntries) = await ExecuteForDBAsync(async dbContext =>
            {
                int amountNewEntries = await dbContext.Offlinemessages
                   .Where(msg => player.Entity != null && msg.TargetId == player.Entity.Id && !msg.Seen)
                   .AsNoTracking()
                   .CountAsync();
                int amountEntries = await dbContext.Offlinemessages
                    .Where(msg => player.Entity != null && msg.TargetId == player.Entity.Id)
                    .AsNoTracking()
                    .CountAsync();
                return (amountNewEntries, amountEntries);
            });
           

            if (amountNewEntries > 0)
            {
                player.SendMessage(string.Format(player.Language.GOT_UNREAD_OFFLINE_MESSAGES, amountNewEntries));
            }
        }
    }
}
