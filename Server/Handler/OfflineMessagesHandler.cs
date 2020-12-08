using System;
using System.Data;
using System.Linq;
using BonusBotConnector.Client;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler
{
    public class OfflineMessagesHandler : DatabaseEntityWrapper
    {
        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public OfflineMessagesHandler(EventsHandler eventsHandler, TDSDbContext dbContext, BonusBotConnectorClient bonusBotConnectorClient,
            ITDSPlayerHandler tdsPlayerHandler)
            : base(dbContext)
        {
            _bonusBotConnectorClient = bonusBotConnectorClient;
            _tdsPlayerHandler = tdsPlayerHandler;

            eventsHandler.PlayerLoggedIn += CheckOfflineMessages;
        }

        public async void Add(int targetId, ulong? targetDiscordId, Players? source, string message)
        {
            try
            {
                var msg = new Offlinemessages()
                {
                    TargetId = targetId,
                    SourceId = source?.Id ?? -1,
                    Message = message
                };

                await ExecuteForDBAsync(async dbContext =>
                {
                    dbContext.Add(msg);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);

                if (targetDiscordId.HasValue)
                    _bonusBotConnectorClient.PrivateChat?.SendOfflineMessage(source?.Discriminator ?? "Source", message, targetDiscordId.Value);

                InformIfPlayerIsOnline(targetId);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public async void Add(Players target, Players? source, string message)
        {
            try
            {
                Offlinemessages msg = new Offlinemessages()
                {
                    TargetId = target.Id,
                    SourceId = source?.Id ?? -1,
                    Message = message
                };

                await ExecuteForDBAsync(async dbContext =>
                {
                    dbContext.Add(msg);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);

                if (target.DiscordUserId.HasValue)
                    _bonusBotConnectorClient.PrivateChat?.SendOfflineMessage(source?.Discriminator ?? "System", message, target.DiscordUserId.Value);

                InformIfPlayerIsOnline(target.Id);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public async void CheckOfflineMessages(ITDSPlayer player)
        {
            try
            {
                (int amountNewEntries, int amountEntries) = await ExecuteForDBAsync(async dbContext =>
                {
                    int amountNewEntries = await dbContext.Offlinemessages
                       .Where(msg => player.Entity != null && msg.TargetId == player.Entity.Id && !msg.Seen)
                       .AsNoTracking()
                       .CountAsync()
                       .ConfigureAwait(false);
                    int amountEntries = await dbContext.Offlinemessages
                        .Where(msg => player.Entity != null && msg.TargetId == player.Entity.Id)
                        .AsNoTracking()
                        .CountAsync()
                        .ConfigureAwait(false);
                    return (amountNewEntries, amountEntries);
                }).ConfigureAwait(false);

                if (amountNewEntries > 0)
                {
                    NAPI.Task.RunSafe(() => 
                        player.SendChatMessage(string.Format(player.Language.GOT_UNREAD_OFFLINE_MESSAGES, amountNewEntries)));
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private void InformIfPlayerIsOnline(int playerId)
        {
            var player = _tdsPlayerHandler.GetPlayer(playerId);
            if (player is { })
                player.SendNotification(player.Language.NEW_OFFLINE_MESSAGE);
        }
    }
}
