﻿using BonusBotConnector.Client;
using BonusBotConnector_Server;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Userpanel.Support;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Userpanel;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums.Userpanel;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Userpanel
{
    public class UserpanelSupportRequestHandler : DatabaseEntityWrapper, IUserpanelSupportRequestHandler
    {
        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly Dictionary<int, HashSet<ITDSPlayer>> _inSupportRequest = new Dictionary<int, HashSet<ITDSPlayer>>();
        private readonly HashSet<ITDSPlayer> _inSupportRequestsList = new HashSet<ITDSPlayer>();

        private readonly ISettingsHandler _settingsHandler;

        public UserpanelSupportRequestHandler(EventsHandler eventsHandler, TDSDbContext dbContext,
            ISettingsHandler settingsHandler, BonusBotConnectorClient bonusBotConnectorClient, BonusBotConnectorServer bonusBotConnectorServer, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(dbContext)
        {
            _settingsHandler = settingsHandler;
            _bonusBotConnectorClient = bonusBotConnectorClient;

            bonusBotConnectorServer.SupportRequestService.AnswerRequestFromDiscord += AnswerRequestFromDiscord;
            bonusBotConnectorServer.SupportRequestService.CreateRequestFromDiscord += CreateRequestFromDiscord;
            bonusBotConnectorServer.SupportRequestService.ToggleClosedRequestFromDiscord += ToggleClosedRequestFromDiscord;

            eventsHandler.Hour += DeleteTooLongClosedRequests;

            eventsHandler.PlayerLoggedOut += (player) =>
            {
                lock (_inSupportRequestsList)
                {
                    _inSupportRequestsList.Remove(player);
                }

                int leftRequestId = -1;
                foreach (var entry in _inSupportRequest)
                {
                    if (entry.Value.Remove(player))
                    {
                        leftRequestId = entry.Key;
                        break;
                    }
                }
                if (leftRequestId >= 0 && _inSupportRequest[leftRequestId].Count == 0)
                {
                    _inSupportRequest.Remove(leftRequestId);
                }
            };

            remoteBrowserEventsHandler.Add(ToServerEvent.GetSupportRequestData, GetSupportRequestData);
            remoteBrowserEventsHandler.Add(ToServerEvent.SetSupportRequestClosed, SetSupportRequestClosed);
            remoteBrowserEventsHandler.Add(ToServerEvent.SendSupportRequest, SendRequest);
            remoteBrowserEventsHandler.Add(ToServerEvent.SendSupportRequestMessage, SendMessage);
            remoteBrowserEventsHandler.Add(ToServerEvent.LeftSupportRequest, LeftSupportRequest);
            remoteBrowserEventsHandler.Add(ToServerEvent.LeftSupportRequestsList, LeftSupportRequestsList);
        }

        public async Task<string?> AnswerRequestFromDiscord(ulong discordUserId, int requestId, string text)
        {
            var playerData = await ExecuteForDBAsync(async dbContext =>
            {
                return await dbContext.Players
                    .Where(p => p.DiscordUserId == discordUserId)
                    .Select(p => new { p.Id, p.Name })
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }).ConfigureAwait(false);
            if (playerData is null)
                return $"There is no player with that Discord-ID.";

            var request = await ExecuteForDBAsync(async dbContext
                => await dbContext.SupportRequests.FirstOrDefaultAsync(r => r.Id == requestId).ConfigureAwait(false)).ConfigureAwait(false);
            if (request is null)
                return "-";

            var maxMessageIndex = await ExecuteForDBAsync(async dbContext
               => await dbContext.SupportRequestMessages
                    .Where(m => m.RequestId == requestId)
                    .MaxAsync(m => m.MessageIndex)
                    .ConfigureAwait(false) + 1)
                .ConfigureAwait(false);

            var messageEntity = new SupportRequestMessages
            {
                AuthorId = playerData.Id,
                MessageIndex = maxMessageIndex,
                RequestId = requestId,
                Text = text
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.SupportRequestMessages.Add(messageEntity);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            if (!_inSupportRequest.ContainsKey(requestId))
                return null;

            string messageJson = Serializer.ToBrowser(new SupportRequestMessage
            {
                Author = playerData.Name,
                Message = messageEntity.Text,
                CreateTime = messageEntity.CreateTime.ToString()
            });

            NAPI.Task.RunSafe(() =>
            {
                foreach (var target in _inSupportRequest[requestId])
                {
                    target.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SyncNewSupportRequestMessage, requestId, messageJson);
                }
            });

            return null;
        }

        public async Task<string?> CreateRequestFromDiscord(ulong discordUserId, string title, string text, SupportType supportType, int atleastAdminLevel)
        {
            var playerId = await ExecuteForDBAsync(async dbContext =>
            {
                return await dbContext.Players
                    .Where(p => p.DiscordUserId == discordUserId)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }).ConfigureAwait(false);
            if (playerId == 0)
                return $"There is no player with that Discord-ID.";

            var requestEntity = new SupportRequests
            {
                AuthorId = playerId,
                AtleastAdminLevel = atleastAdminLevel,
                Messages = new List<SupportRequestMessages>
                {
                    new SupportRequestMessages
                    {
                        AuthorId = playerId,
                        MessageIndex = 0,
                        Text = text
                    }
                },
                Title = title,
                Type = supportType
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.SupportRequests.Add(requestEntity);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            return requestEntity.Id.ToString();
        }

        public async void DeleteTooLongClosedRequests(int _)
        {
            try
            {
                var deleteAfterDays = _settingsHandler.ServerSettings.DeleteRequestsDaysAfterClose;
                var requestIdsToDelete = await ExecuteForDBAsync(async dbContext =>
                {
                    var requests = await dbContext.SupportRequests
                        .Where(r => r.CloseTime != null && r.CloseTime.Value.AddDays(deleteAfterDays) < DateTime.UtcNow)
                        .ToListAsync()
                        .ConfigureAwait(false);
                    if (requests.Count == 0)
                        return null;

                    var requestIdsToDelete = requests.Select(r => r.Id);
                    dbContext.SupportRequests.RemoveRange(requests);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);

                    return requestIdsToDelete;
                }).ConfigureAwait(false);

                if (requestIdsToDelete is { })
                    _bonusBotConnectorClient.Support?.Delete(requestIdsToDelete);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private async Task<object?> GetSupportRequestData(RemoteBrowserEventArgs args)
        {
            var player = args.Player;
            try
            {
                if (args.Args.Count == 0)
                    return null;
                int? requestId;
                if ((requestId = Utils.GetInt(args.Args[0])) == null)
                    return null;

                var data = await ExecuteForDBAsync(async dbContext
                    => await dbContext.SupportRequests
                        .Include(r => r.Messages)
                        .ThenInclude(m => m.Author)
                        .Where(r => r.Id == requestId)
                        .Select(r => new SupportRequestData
                        {
                            ID = r.Id,
                            Title = r.Title,
                            Messages = r.Messages.Select(m => new SupportRequestMessageData
                            {
                                Author = m.Author.Name,
                                Message = m.Text,
                                CreateTimeDate = m.CreateTime
                            }),
                            Type = r.Type,
                            AtleastAdminLevel = r.AtleastAdminLevel,
                            Closed = r.CloseTime != null,

                            AuthorId = r.AuthorId
                        })
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false))
                    .ConfigureAwait(false);

                if (data is null)
                    return null;
                if (data.AuthorId != player.Entity!.Id && player.Admin.Level.Level == 0)
                    return null;

                foreach (var entry in data.Messages)
                {
                    entry.CreateTime = player.Timezone.GetLocalDateTimeString(entry.CreateTimeDate);
                }

                if (!_inSupportRequest.ContainsKey(data.ID))
                    _inSupportRequest[data.ID] = new HashSet<ITDSPlayer>();
                _inSupportRequest[data.ID].Add(player);

                return Serializer.ToBrowser(data);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex, player);
                return null;
            }
        }

        public async Task<string?> GetSupportRequests(ITDSPlayer player)
        {
            var data = await ExecuteForDBAsync(async dbContext
                => await dbContext.SupportRequests
                    .Include(r => r.Author)
                    .Where(r => r.AuthorId == player.Entity!.Id
                        || player.Admin.Level.Level > 0)
                    .Select(r => new SupportRequestsListData
                    {
                        ID = r.Id,
                        PlayerName = r.Author.Name,
                        CreateTimeDate = r.CreateTime,
                        Title = r.Title,
                        Type = r.Type,
                        Closed = r.CloseTime != null
                    })
                    .ToListAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            foreach (var entry in data)
            {
                entry.CreateTime = player.Timezone.GetLocalDateTimeString(entry.CreateTimeDate);
            }

            lock (_inSupportRequestsList)
                _inSupportRequestsList.Add(player);

            return Serializer.ToBrowser(data);
        }

        private object? LeftSupportRequest(RemoteBrowserEventArgs args)
        {
            int requestId = (int)args.Args[0];

            if (!_inSupportRequest.ContainsKey(requestId))
                return null;

            if (_inSupportRequest[requestId].Remove(args.Player) && _inSupportRequest[requestId].Count == 0)
            {
                _inSupportRequest.Remove(requestId);
            }
            return null;
        }

        private object? LeftSupportRequestsList(RemoteBrowserEventArgs args)
        {
            lock (_inSupportRequestsList)
                _inSupportRequestsList.Remove(args.Player);
            return null;
        }

        private async Task<object?> SendMessage(RemoteBrowserEventArgs args)
        {
            int? requestId = Utils.GetInt(args.Args[0]);
            if (requestId is null)
                return null;

            var player = args.Player;
            string message = (string)args.Args[1];

            var request = await ExecuteForDBAsync(async dbContext
                => await dbContext.SupportRequests
                    .FirstOrDefaultAsync(r => r.Id == requestId)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (request is null)
                return null;
            if (request.AuthorId != player.Entity!.Id && player.Admin.Level.Level == 0)
                return null;

            var maxMessageIndex = await ExecuteForDBAsync(async dbContext
                => await dbContext.SupportRequestMessages
                    .Where(m => m.RequestId == requestId)
                    .MaxAsync(m => m.MessageIndex)
                    .ConfigureAwait(false) + 1)
                .ConfigureAwait(false);

            var messageEntity = new SupportRequestMessages
            {
                AuthorId = player.Entity.Id,
                MessageIndex = maxMessageIndex,
                RequestId = requestId.Value,
                Text = message
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.SupportRequestMessages.Add(messageEntity);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            _bonusBotConnectorClient.Support?.Answer(player, messageEntity);

            if (!_inSupportRequest.ContainsKey(requestId.Value))
                return null;

            string messageJson = Serializer.ToBrowser(new SupportRequestMessage
            {
                Author = player.DisplayName,
                Message = messageEntity.Text,
                CreateTime = player.Timezone.GetLocalDateTimeString(messageEntity.CreateTime)
            });

            NAPI.Task.RunSafe(() =>
            {
                foreach (var target in _inSupportRequest[requestId.Value])
                {
                    target.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SyncNewSupportRequestMessage, requestId, messageJson);
                }
            });

            return null;
        }

        private async Task<object?> SendRequest(RemoteBrowserEventArgs args)
        {
            var player = args.Player;
            string json = (string)args.Args[0];

            var request = Serializer.FromBrowser<SupportRequestData>(json);
            if (request is null)
                return null;

            var requestEntity = new SupportRequests
            {
                AuthorId = player.Entity!.Id,
                AtleastAdminLevel = request.AtleastAdminLevel,
                Messages = new List<SupportRequestMessages>
                {
                    new SupportRequestMessages
                    {
                        AuthorId = player.Entity!.Id,
                        MessageIndex = 0,
                        Text = request.Messages.First().Message
                    }
                },
                Title = request.Title,
                Type = request.Type
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.SupportRequests.Add(requestEntity);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            _bonusBotConnectorClient.Support?.Create(player, requestEntity);

            NAPI.Task.RunSafe(() => player.SendNotification(player.Language.SUPPORT_REQUEST_CREATED));
            return null;
        }

        private async Task<object?> SetSupportRequestClosed(RemoteBrowserEventArgs args)
        {
            int? requestId = Utils.GetInt(args.Args[0]);
            if (requestId == null)
                return null;
            bool closed = (bool)args.Args[1];

            var request = await ExecuteForDBAsync(async dbContext
                => await dbContext.SupportRequests
                    .FirstOrDefaultAsync(r => r.Id == requestId)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (request is null)
                return null;
            if (request.AuthorId != args.Player.Entity!.Id && args.Player.Admin.Level.Level == 0)
                return null;
            if (request.CloseTime is { } && closed || request.CloseTime is null && !closed)
                return null;

            if (closed)
                request.CloseTime = DateTime.UtcNow;
            else
                request.CloseTime = null;

            await ExecuteForDBAsync(async dbContext => await dbContext.SaveChangesAsync()).ConfigureAwait(false);

            var playersToTriggerTo = _inSupportRequestsList.ToList();
            NAPI.Task.RunSafe(() =>
            {
                foreach (var target in playersToTriggerTo)
                {
                    target.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetSupportRequestClosed, requestId, closed);
                }
            });

            _bonusBotConnectorClient.Support?.ToggleClosed(args.Player, request.Id, closed);
            return null;
        }

        public async Task<string?> ToggleClosedRequestFromDiscord(ulong discordUserId, int requestId, bool closed)
        {
            var request = await ExecuteForDBAsync(async dbContext
                => await dbContext.SupportRequests
                    .FirstOrDefaultAsync(r => r.Id == requestId)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (request is null)
                return null;
            if (request.CloseTime is { } && closed || request.CloseTime is null && !closed)
                return null;

            if (closed)
                request.CloseTime = DateTime.UtcNow;
            else
                request.CloseTime = null;

            await ExecuteForDBAsync(async dbContext
                => await dbContext
                    .SaveChangesAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            var playersToTriggerTo = _inSupportRequestsList.ToList();
            NAPI.Task.RunSafe(() =>
            {
                foreach (var target in playersToTriggerTo)
                {
                    target.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetSupportRequestClosed, requestId, closed);
                }
            });

            return null;
        }
    }
}