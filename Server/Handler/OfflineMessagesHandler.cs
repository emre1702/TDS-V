using System;
using System.Data;
using System.Linq;
using BonusBotConnector.Client;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.Handler
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

        public async void Add(int targetId, ulong? targetDiscordId, Players source, string message)
        {
            try
            {
                var msg = new Offlinemessages()
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

                if (targetDiscordId.HasValue)
                    _bonusBotConnectorClient.PrivateChat?.SendOfflineMessage(source.GetDiscriminator(), message, targetDiscordId.Value);

                InformIfPlayerIsOnline(targetId);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public async void Add(Players target, Players source, string message)
        {
            try
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

                if (target.PlayerSettings.DiscordUserId.HasValue)
                    _bonusBotConnectorClient.PrivateChat?.SendOfflineMessage(source.GetDiscriminator(), message, target.PlayerSettings.DiscordUserId.Value);

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
                       .CountAsync();
                    int amountEntries = await dbContext.Offlinemessages
                        .Where(msg => player.Entity != null && msg.TargetId == player.Entity.Id)
                        .AsNoTracking()
                        .CountAsync();
                    return (amountNewEntries, amountEntries);
                });

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
            var player = _tdsPlayerHandler.Get(playerId);
            if (player is { })
                player.SendNotification(player.Language.NEW_OFFLINE_MESSAGE);
        }
    }
}
