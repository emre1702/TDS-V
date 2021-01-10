using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Userpanel.OfflineMessage;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Userpanel
{
    public class UserpanelOfflineMessagesHandler : DatabaseEntityWrapper, IUserpanelOfflineMessagesHandler
    {
        private readonly OfflineMessagesHandler _offlineMessagesHandler;
        private readonly ISettingsHandler _settingsHandler;

        public UserpanelOfflineMessagesHandler(TDSDbContext dbContext, ISettingsHandler settingsHandler, OfflineMessagesHandler offlineMessagesHandler,
            EventsHandler eventsHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(dbContext)
        {
            (_settingsHandler, _offlineMessagesHandler) = (settingsHandler, offlineMessagesHandler);

            eventsHandler.Hour += DeleteOldMessages;

            remoteBrowserEventsHandler.Add(ToServerEvent.AnswerToOfflineMessage, Answer);
            remoteBrowserEventsHandler.Add(ToServerEvent.SendOfflineMessage, Send);
            remoteBrowserEventsHandler.Add(ToServerEvent.DeleteOfflineMessage, Delete);
        }

        private async Task<object?> Answer(RemoteBrowserEventArgs args)
        {
            int? offlineMessageID;
            if ((offlineMessageID = Utils.GetInt(args.Args[0])) is null)
                return null;
            string? message = Convert.ToString(args.Args[1]);
            if (message is null)
                return null;

            var offlineMessage = await ExecuteForDBAsync(async dbContext =>
                await dbContext.Offlinemessages
                    .Include(o => o.Source)
                    .ThenInclude(s => s.PlayerSettings)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == offlineMessageID)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (offlineMessage is null)
                return null;

            _offlineMessagesHandler.Add(offlineMessage.Source, args.Player.Entity!, message);

            return null;
        }

        private async Task<object?> Delete(RemoteBrowserEventArgs args)
        {
            int? offlineMessageId;
            if ((offlineMessageId = Utils.GetInt(args.Args[0])) is null)
                return null;

            await ExecuteForDBAsync(async dbContext =>
            {
                var offlineMessage = await dbContext.Offlinemessages.FirstOrDefaultAsync(o => o.Id == offlineMessageId).ConfigureAwait(false);
                if (offlineMessage is null)
                    return;
                dbContext.Offlinemessages.Remove(offlineMessage);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            return null;
        }

        public async void DeleteOldMessages(int _)
        {
            try
            {
                var deleteAfterDays = _settingsHandler.ServerSettings.DeleteOfflineMessagesAfterDays;
                await ExecuteForDBAsync(async dbContext =>
                {
                    var msgs = await dbContext.Offlinemessages.Where(o => o.Timestamp.AddDays(deleteAfterDays) < DateTime.UtcNow).ToListAsync().ConfigureAwait(false);
                    dbContext.Offlinemessages.RemoveRange(msgs);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public async Task<string?> GetData(ITDSPlayer player)
        {
            var offlineMessages = await ExecuteForDBAsync(async dbContext
                => await dbContext.Offlinemessages
                    .Where(o => o.TargetId == player.Entity!.Id)
                    .Include(o => o.Source)
                    .Select(o => new OfflineMessageDto
                    {
                        ID = o.Id,
                        PlayerName = o.Source.Name,
                        CreateTimeDate = o.Timestamp,
                        Text = o.Message,
                        Seen = o.Seen
                    })
                    .ToListAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            foreach (var message in offlineMessages)
            {
                message.CreateTime = player.Timezone.GetLocalDateTimeString(message.CreateTimeDate);
            }

            string json = Serializer.ToBrowser(offlineMessages);

            if (offlineMessages.Any(m => !m.Seen))
            {
                await ExecuteForDBAsync(async dbContext =>
                {
                    foreach (var message in offlineMessages.Where(m => !m.Seen))
                    {
                        message.Seen = true;
                    }
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }

            return json;
        }

        private async Task<object?> Send(RemoteBrowserEventArgs args)
        {
            string? playerName = Convert.ToString(args.Args[0]);
            if (playerName is null)
                return false;
            int? targetId;
            if (!(targetId = Utils.GetInt(args.Args[0])).HasValue)
            {
                targetId = await ExecuteForDBAsync(async dbContext
                    => await dbContext.Players
                        .Where(p => p.Name == playerName || p.SCName == playerName)
                        .Select(p => p.Id)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false))
                    .ConfigureAwait(false);
                if (targetId is null || targetId == 0)
                {
                    args.Player.SendNotification(args.Player.Language.PLAYER_DOESNT_EXIST, true);
                    return false;
                }
            }

            string? message = Convert.ToString(args.Args[1]);
            if (message is null)
                return false;

            var discordUserId = await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players
                    .Where(p => p.Id == targetId.Value)
                    .Select(p => p.DiscordUserId)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            _offlineMessagesHandler.Add(targetId.Value, discordUserId, args.Player.Entity!, message);

            return true;
        }
    }
}