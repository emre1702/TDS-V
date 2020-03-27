﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Player;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Userpanel
{
    class UserpanelOfflineMessagesHandler : DatabaseEntityWrapper
    {
        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;
        private readonly OfflineMessagesHandler _offlineMessagesHandler;

        public UserpanelOfflineMessagesHandler(TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer,
            ISettingsHandler settingsHandler, OfflineMessagesHandler offlineMessagesHandler, EventsHandler eventsHandler) 
            : base(dbContext, loggingHandler)
        {
            (_serializer, _settingsHandler, _offlineMessagesHandler) = (serializer, settingsHandler, offlineMessagesHandler);

            eventsHandler.Hour += DeleteOldMessages;
        }

        public async Task<string?> GetData(ITDSPlayer player)
        {
            var offlineMessages = await ExecuteForDBAsync(async dbContext 
                => await dbContext.Offlinemessages
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
                    .ToListAsync());

            foreach (var message in offlineMessages)
            {
                message.CreateTime = player.GetLocalDateTimeString(message.CreateTimeDate);
            }

            string json = _serializer.ToBrowser(offlineMessages);

            if (offlineMessages.Any(m => !m.Seen))
            {
                await ExecuteForDBAsync(async dbContext =>
                {
                    foreach (var message in offlineMessages.Where(m => !m.Seen))
                    {
                        message.Seen = true;
                    }
                    await dbContext.SaveChangesAsync();
                });
                
            }

            return json;
        }

        public async Task<object?> Answer(ITDSPlayer player, object[] args)
        {
            int? offlineMessageID;
            if ((offlineMessageID = Utils.GetInt(args[0])) is null)
                return null;
            string? message = Convert.ToString(args[1]);
            if (message is null)
                return null;

            var offlineMessage = await ExecuteForDBAsync(async dbContext =>
                await dbContext.Offlinemessages
                    .Include(o => o.Source)
                    .ThenInclude(s => s.PlayerSettings)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == offlineMessageID));
            if (offlineMessage is null)
                return null;

            _offlineMessagesHandler.AddOfflineMessage(offlineMessage.Source, player.Entity!, message);

            return null;
        }

        public async Task<object?> Send(ITDSPlayer player, object[] args)
        {
            string? playerName = Convert.ToString(args[0]);
            if (playerName is null)
                return false;
            int? targetId;
            if (!(targetId = Utils.GetInt(args[1])).HasValue)
            {
                targetId = await ExecuteForDBAsync(async dbContext 
                    => await dbContext.Players.Where(p => p.Name == playerName || p.SCName == playerName).Select(p => p.Id).FirstOrDefaultAsync());
                if (targetId is null || targetId == 0)
                {
                    player.SendNotification(player.Language.PLAYER_DOESNT_EXIST, true);
                    return false;
                }
            }

            string? message = Convert.ToString(args[1]);
            if (message is null)
                return false;


            var discordUserId = await ExecuteForDBAsync(async dbContext =>
                await dbContext.PlayerSettings
                    .Where(p => p.PlayerId == targetId.Value)
                    .Select(p => p.DiscordUserId)
                    .FirstOrDefaultAsync());

            _offlineMessagesHandler.Add(targetId.Value, discordUserId, player.Entity!, message);

            return true;
        }

        public async Task<object?> Delete(ITDSPlayer player, object[] args)
        {
            int? offlineMessageId;
            if ((offlineMessageId = Utils.GetInt(args[0])) is null)
                return null;

            await ExecuteForDBAsync(async dbContext =>
            {
                var offlineMessage = await dbContext.Offlinemessages.FirstOrDefaultAsync(o => o.Id == offlineMessageId);
                if (offlineMessage is null)
                    return;
                dbContext.Offlinemessages.Remove(offlineMessage);

                await dbContext.SaveChangesAsync();
            });
           
            return null;
        }

        public async void DeleteOldMessages(ulong _)
        {
            var deleteAfterDays = _settingsHandler.ServerSettings.DeleteOfflineMessagesAfterDays;
            await ExecuteForDBAsync(async dbContext => 
                await dbContext.Offlinemessages.Where(o => o.Timestamp.AddDays(deleteAfterDays) < DateTime.UtcNow).DeleteFromQueryAsync());
        }
    }

    public class OfflineMessage
    {
        [JsonProperty("0")]
        public int ID { get; set; }
        [JsonProperty("1")]
        public string PlayerName { get; set; } = string.Empty;
        [JsonProperty("2")]
        public string CreateTime { get; set; } = string.Empty;
        [JsonProperty("3")]
        public string Text { get; set; } = string.Empty;
        [JsonProperty("4")]
        public bool Seen { get; set; }

        [JsonIgnore]
        public DateTime CreateTimeDate { get; set; }
    }
}