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
using TDS_Server.Data;
using TDS_Server.Data.Defaults;

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

        public async Task<object?> GetSupportRequestData(ITDSPlayer player, object[] args)
        {
            try
            {
                if (args.Length == 0)
                    return null;
                int? requestId;
                if ((requestId = Utils.GetInt(args[0])) == null)
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
                        .FirstOrDefaultAsync());

                if (data is null)
                    return null;
                if (data.AuthorId != player.Entity!.Id && player.AdminLevel.Level == 0)
                    return null;

                foreach (var entry in data.Messages)
                {
                    entry.CreateTime = player.GetLocalDateTimeString(entry.CreateTimeDate);
                }

                if (!_inSupportRequest.ContainsKey(data.ID))
                    _inSupportRequest[data.ID] = new HashSet<ITDSPlayer>();
                _inSupportRequest[data.ID].Add(player);

                return _serializer.ToBrowser(data);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, player);
                return null;
            }
        }

        public async Task<object?> SendRequest(ITDSPlayer player, object[] args)
        {
            string json = (string)args[0];

            var request = _serializer.FromBrowser<SupportRequestData>(json);
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

                await dbContext.SaveChangesAsync();
            });
                
            player.SendNotification(player.Language.SUPPORT_REQUEST_CREATED);
            return null;
        }

        public async Task<object?> SendMessage(ITDSPlayer player, object[] args)
        {
            int? requestId = Utils.GetInt(args[0]);
            if (requestId is null)
                return null;

            string message = (string)args[1];

            var request = await ExecuteForDBAsync(async dbContext
                => await dbContext.SupportRequests.FirstOrDefaultAsync(r => r.Id == requestId));
            if (request is null)
                return null;
            if (request.AuthorId != player.Entity!.Id && player.AdminLevel.Level == 0)
                return null;

            var maxMessageIndex = await ExecuteForDBAsync(async dbContext 
                => await dbContext.SupportRequestMessages.Where(m => m.RequestId == requestId).MaxAsync(m => m.MessageIndex) + 1);

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
                await dbContext.SaveChangesAsync();
            });
          
            if (!_inSupportRequest.ContainsKey(requestId.Value))
                return null;

            string messageJson = _serializer.ToBrowser(new SupportRequestMessage
            {
                Author = player.DisplayName,
                Message = messageEntity.Text,
                CreateTime = player.GetLocalDateTimeString(messageEntity.CreateTime)
            });

            foreach (var target in _inSupportRequest[requestId.Value])
            {
                target.SendEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SyncNewSupportRequestMessage, requestId, messageJson);
            }
            return null;
        }

        public async Task<object?> SetSupportRequestClosed(ITDSPlayer player, object[] args)
        {
            int? requestId = Utils.GetInt(args[0]);
            if (requestId == null)
                return null;
            bool closed = (bool)args[1];

            var request = await ExecuteForDBAsync(async dbContext 
                => await dbContext.SupportRequests.FirstOrDefaultAsync(r => r.Id == requestId));
            if (request is null)
                return null;
            if (request.AuthorId != player.Entity!.Id && player.AdminLevel.Level == 0)
                return null;
            if (request.CloseTime is { } && closed || request.CloseTime is null && !closed)
                return null;

            if (closed)
                request.CloseTime = DateTime.UtcNow;
            else
                request.CloseTime = null;

            await ExecuteForDBAsync(async dbContext => await dbContext.SaveChangesAsync());

            foreach (var target in _inSupportRequestsList)
            {
                target.SendEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetSupportRequestClosed, requestId, closed);
            }
            return null;
        }

        public object? LeftSupportRequestsList(ITDSPlayer player, object[] _)
        {
            _inSupportRequestsList.Remove(player);
            return null;
        }

        public object? LeftSupportRequest(ITDSPlayer player, object[] args)
        {
            int requestId = (int)args[0];

            if (!_inSupportRequest.ContainsKey(requestId))
                return null;

            if (_inSupportRequest[requestId].Remove(player) && _inSupportRequest[requestId].Count == 0)
            {
                _inSupportRequest.Remove(requestId);
            }
            return null;
        }

        public async void DeleteTooLongClosedRequests(int _)
        {

            var deleteAfterDays = _settingsHandler.ServerSettings.DeleteRequestsDaysAfterClose;
            await ExecuteForDBAsync(async dbContext =>
            {
                var requests = await dbContext.SupportRequests
                    .Where(r => r.CloseTime != null && r.CloseTime.Value.AddDays(deleteAfterDays) < DateTime.UtcNow)
                    .ToListAsync();
                dbContext.SupportRequests.RemoveRange(requests);
                await dbContext.SaveChangesAsync();
            });
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
