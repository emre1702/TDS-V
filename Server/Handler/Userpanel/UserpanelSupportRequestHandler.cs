using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Userpanel;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Default;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSupportRequestHandler : DatabaseEntityWrapper
    {
        private readonly HashSet<ITDSPlayer> _inSupportRequestsList = new HashSet<ITDSPlayer>();
        private readonly Dictionary<int, HashSet<ITDSPlayer>> _inSupportRequest = new Dictionary<int, HashSet<ITDSPlayer>>();

        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;

        public UserpanelSupportRequestHandler(EventsHandler eventsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, ISettingsHandler settingsHandler)
            : base(dbContext, loggingHandler)
        {
            _serializer = serializer;
            _settingsHandler = settingsHandler;

            eventsHandler.Hour += DeleteTooLongClosedRequests;

            eventsHandler.PlayerLoggedOut += (player) =>
            {
                _inSupportRequestsList.Remove(player);
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
        }

        public async Task<string?> GetSupportRequests(ITDSPlayer player)
        {

            var data = await ExecuteForDBAsync(async dbContext 
                => await dbContext.SupportRequests
                    .Include(r => r.Author)
                    .Where(r => r.AuthorId == player.Entity!.Id
                        || player.AdminLevel.Level > 0)
                    .Select(r => new SupportRequestsListData
                    {
                        ID = r.Id,
                        PlayerName = r.Author.Name,
                        CreateTimeDate = r.CreateTime,
                        Title = r.Title,
                        Type = r.Type,
                        Closed = r.CloseTime != null
                    })
                    .ToListAsync());

            foreach (var entry in data)
            {
                entry.CreateTime = player.GetLocalDateTimeString(entry.CreateTimeDate);
            }

            _inSupportRequestsList.Add(player);

            return _serializer.ToBrowser(data);
        }

        public async Task GetSupportRequestData(ITDSPlayer player, int requestId)
        {
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
                    .FirstOrDefaultAsync());

            if (data is null)
                return;
            if (data.AuthorId != player.Entity!.Id && player.AdminLevel.Level == 0)
                return;

            foreach (var entry in data.Messages)
            {
                entry.CreateTime = player.GetLocalDateTimeString(entry.CreateTimeDate);
            }

            if (!_inSupportRequest.ContainsKey(data.ID))
                _inSupportRequest[data.ID] = new HashSet<ITDSPlayer>();
            _inSupportRequest[data.ID].Add(player);

            player.SendEvent(ToClientEvent.GetSupportRequestData, _serializer.ToBrowser(data));
        }

        public async Task SendRequest(ITDSPlayer player, string json)
        {
            try
            {
                var request = _serializer.FromBrowser<SupportRequestData>(json);
                if (request is null)
                    return;

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

                    await dbContext.SaveChangesAsync();
                });
                
                player.SendNotification(player.Language.SUPPORT_REQUEST_CREATED);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError("SendRequest failed: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, player);
            }
        }

        public async Task SendMessage(ITDSPlayer player, int requestId, string message)
        {

            var request = await ExecuteForDBAsync(async dbContext
                => await dbContext.SupportRequests.FirstOrDefaultAsync(r => r.Id == requestId));
            if (request is null)
                return;
            if (request.AuthorId != player.Entity!.Id && player.AdminLevel.Level == 0)
                return;

            var maxMessageIndex = await ExecuteForDBAsync(async dbContext 
                => await dbContext.SupportRequestMessages.Where(m => m.RequestId == requestId).MaxAsync(m => m.MessageIndex) + 1);

            var messageEntity = new SupportRequestMessages
            {
                AuthorId = player.Entity.Id,
                MessageIndex = maxMessageIndex,
                RequestId = requestId,
                Text = message
            };

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.SupportRequestMessages.Add(messageEntity);
                await dbContext.SaveChangesAsync();
            });
          
            if (!_inSupportRequest.ContainsKey(requestId))
                return;

            string messageJson = _serializer.ToBrowser(new SupportRequestMessage
            {
                Author = player.DisplayName,
                Message = messageEntity.Text,
                CreateTime = player.GetLocalDateTimeString(messageEntity.CreateTime)
            });

            foreach (var target in _inSupportRequest[requestId])
            {
                target.SendEvent(ToClientEvent.SyncNewSupportRequestMessage, requestId, messageJson);
            }
        }

        public async Task SetSupportRequestClosed(ITDSPlayer player, int requestId, bool closed)
        {

            var request = await ExecuteForDBAsync(async dbContext 
                => await dbContext.SupportRequests.FirstOrDefaultAsync(r => r.Id == requestId));
            if (request is null)
                return;
            if (request.AuthorId != player.Entity!.Id && player.AdminLevel.Level == 0)
                return;
            if (request.CloseTime is { } && closed || request.CloseTime is null && !closed)
                return;

            if (closed)
                request.CloseTime = DateTime.UtcNow;
            else
                request.CloseTime = null;

            await ExecuteForDBAsync(async dbContext => await dbContext.SaveChangesAsync());

            foreach (var target in _inSupportRequestsList)
            {
                target.SendEvent(ToClientEvent.SetSupportRequestClosed, requestId, closed);
            }
        }

        public void LeftSupportRequestsList(ITDSPlayer player)
        {
            _inSupportRequestsList.Remove(player);
        }

        public void LeftSupportRequest(ITDSPlayer player, int requestId)
        {
            if (!_inSupportRequest.ContainsKey(requestId))
                return;

            if (_inSupportRequest[requestId].Remove(player) && _inSupportRequest[requestId].Count == 0)
            {
                _inSupportRequest.Remove(requestId);
            }
        }

        public async void DeleteTooLongClosedRequests(ulong _)
        {

            var deleteAfterDays = _settingsHandler.ServerSettings.DeleteRequestsDaysAfterClose;
            await ExecuteForDBAsync(async dbContext 
                => await dbContext.SupportRequests
                    .Where(r => r.CloseTime != null && r.CloseTime.Value.AddDays(deleteAfterDays) < DateTime.UtcNow)
                    .DeleteFromQueryAsync());
        }
    }

    public class SupportRequestsListData
    {
        [JsonProperty("0")]
        public int ID { get; set; }
        [JsonProperty("1")]
        public string PlayerName { get; set; } = string.Empty;
        [JsonProperty("2")]
        public string CreateTime { get; set; } = string.Empty;
        [JsonProperty("3")]
        public SupportType Type { get; set; }
        [JsonProperty("4")]
        public string Title { get; set; } = string.Empty;
        [JsonProperty("5")]
        public bool Closed { get; set; }

        [JsonIgnore]
        public DateTime CreateTimeDate { get; set; }
    }

    public class SupportRequestData
    {
        [JsonProperty("0")]
        public int ID { get; set; }
        [JsonProperty("1")]
        public SupportType Type { get; set; }
        [JsonProperty("2")]
        public string Title { get; set; } = string.Empty;
        [JsonProperty("3")]
        public IEnumerable<SupportRequestMessageData> Messages { get; set; } = new List<SupportRequestMessageData>();
        [JsonProperty("4")]
        public int AtleastAdminLevel { get; set; }
        [JsonProperty("5")]
        public bool Closed { get; set; }

        [JsonIgnore]
        public DateTime CreateTimeDate { get; set; }
        [JsonIgnore]
        public int AuthorId { get; set; }
    }

    public class SupportRequestMessageData
    {
        [JsonProperty("0")]
        public string Author { get; set; } = string.Empty;
        [JsonProperty("1")]
        public string Message { get; set; } = string.Empty;
        [JsonProperty("2")]
        public string CreateTime { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime CreateTimeDate { get; set; }
    }
    public class SupportRequestMessage
    {
        [JsonProperty("0")]
        public string? Author { get; set; }
        [JsonProperty("1")]
        public string? Message { get; set; }
        [JsonProperty("2")]
        public string? CreateTime { get; set; }
    }
}
