using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.EventManager;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Userpanel;

namespace TDS_Server.Core.Manager.Userpanel
{
    static class SupportRequest
    {
        private static readonly HashSet<TDSPlayer> _inSupportRequestsList = new HashSet<TDSPlayer>();
        private static readonly Dictionary<int, HashSet<TDSPlayer>> _inSupportRequest = new Dictionary<int, HashSet<TDSPlayer>>();

        static SupportRequest()
        {
            CustomEventManager.OnPlayerLoggedOut += (player) =>
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

        public static async Task<string?> GetSupportRequests(TDSPlayer player)
        {
            using var dbContext = new TDSDbContext();

            var data = await dbContext.SupportRequests
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
                .ToListAsync();

            foreach (var entry in data)
            {
                entry.CreateTime = player.GetLocalDateTimeString(entry.CreateTimeDate);
            }

            _inSupportRequestsList.Add(player);

            return Serializer.ToBrowser(data);
        }

        public static async Task GetSupportRequestData(TDSPlayer player, int requestId)
        {
            using var dbContext = new TDSDbContext();

            var data = await dbContext.SupportRequests
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
                .FirstOrDefaultAsync();

            if (data is null)
                return;
            if (data.AuthorId != player.Entity!.Id && player.AdminLevel.Level == 0)
                return;

            foreach (var entry in data.Messages)
            {
                entry.CreateTime = player.GetLocalDateTimeString(entry.CreateTimeDate);
            }

            if (!_inSupportRequest.ContainsKey(data.ID))
                _inSupportRequest[data.ID] = new HashSet<TDSPlayer>();
            _inSupportRequest[data.ID].Add(player);

            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.GetSupportRequestData, Serializer.ToBrowser(data)); 
        }

        public static async Task SendRequest(TDSPlayer player, string json)
        {
            try
            {
                var request = Serializer.FromBrowser<SupportRequestData>(json);
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

                using var dbContext = new TDSDbContext();
                dbContext.SupportRequests.Add(requestEntity);

                await dbContext.SaveChangesAsync();   
                
                player.SendNotification(player.Language.SUPPORT_REQUEST_CREATED);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log("SendRequest failed: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, player);
            }
        }

        public static async Task SendMessage(TDSPlayer player, int requestId, string message)
        {
            using var dbContext = new TDSDbContext();

            var request = await dbContext.SupportRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request is null) 
                return;
            if (request.AuthorId != player.Entity!.Id && player.AdminLevel.Level == 0)
                return;

            var maxMessageIndex = await dbContext.SupportRequestMessages.Where(m => m.RequestId == requestId).MaxAsync(m => m.MessageIndex) + 1;

            var messageEntity = new SupportRequestMessages
            {
                AuthorId = player.Entity.Id,
                MessageIndex = maxMessageIndex,
                RequestId = requestId, 
                Text = message
            };
            dbContext.SupportRequestMessages.Add(messageEntity);
            await dbContext.SaveChangesAsync();

            if (!_inSupportRequest.ContainsKey(requestId))
                return;

            string messageJson = Serializer.ToBrowser(new SupportRequestMessage
            {
                Author = player.DisplayName,
                Message = messageEntity.Text,
                CreateTime = player.GetLocalDateTimeString(messageEntity.CreateTime)
            });

            foreach (var target in _inSupportRequest[requestId])
            {
                NAPI.ClientEvent.TriggerClientEvent(target.Player, DToClientEvent.SyncNewSupportRequestMessage, requestId, messageJson);
            }
        }

        public static async Task SetSupportRequestClosed(TDSPlayer player, int requestId, bool closed)
        {
            using var dbContext = new TDSDbContext();

            var request = await dbContext.SupportRequests.FirstOrDefaultAsync(r => r.Id == requestId);
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

            await dbContext.SaveChangesAsync();

            foreach (var target in _inSupportRequestsList)
            {
                NAPI.ClientEvent.TriggerClientEvent(target.Player, DToClientEvent.SetSupportRequestClosed, requestId, closed);
            }
        }

        public static void LeftSupportRequestsList(TDSPlayer player)
        {
            _inSupportRequestsList.Remove(player);
        }

        public static void LeftSupportRequest(TDSPlayer player, int requestId)
        {
            if (!_inSupportRequest.ContainsKey(requestId))
                return;

            if (_inSupportRequest[requestId].Remove(player) && _inSupportRequest[requestId].Count == 0)
            {
                _inSupportRequest.Remove(requestId);
            }
        }

        public static async Task DeleteTooLongClosedRequests()
        {
            using var dbContext = new TDSDbContext();

            var deleteAfterDays = SettingsManager.ServerSettings.DeleteRequestsDaysAfterClose;
            await dbContext.SupportRequests
                .Where(r => r.CloseTime != null && r.CloseTime.Value.AddDays(deleteAfterDays) < DateTime.UtcNow)
                .DeleteFromQueryAsync();
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
        public ESupportType Type { get; set; }
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
        public ESupportType Type { get; set; }
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
