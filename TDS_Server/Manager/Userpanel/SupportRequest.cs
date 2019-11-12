using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum.Userpanel;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Userpanel;

namespace TDS_Server.Manager.Userpanel
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
            using var dbContext = new TDSNewContext();

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

            return JsonConvert.SerializeObject(data);
        }

        public static async Task GetSupportRequestData(TDSPlayer player, int requestId)
        {
            using var dbContext = new TDSNewContext();

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

            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.GetSupportRequestData, JsonConvert.SerializeObject(data)); 
        }

        public static async Task SendRequest(TDSPlayer player, string json)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<SupportRequestData>(json);
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

                using var dbContext = new TDSNewContext();
                dbContext.SupportRequests.Add(requestEntity);

                await dbContext.SaveChangesAsync();   
                
                NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.SUPPORT_REQUEST_CREATED);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log("SendRequest failed: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, player);
            }
        }

        public static async Task SendMessage(TDSPlayer player, int requestId, string message)
        {
            using var dbContext = new TDSNewContext();

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

            string messageJson = JsonConvert.SerializeObject(new 
            {
                Author = player.Client.Name,
                Message = messageEntity.Text,
                CreateTime = player.GetLocalDateTimeString(messageEntity.CreateTime)
            });

            foreach (var target in _inSupportRequest[requestId])
            {
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.SyncNewSupportRequestMessage, requestId, messageJson);
            }
        }

        public static async Task SetSupportRequestClosed(TDSPlayer player, int requestId, bool closed)
        {
            using var dbContext = new TDSNewContext();

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
                NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.SetSupportRequestClosed, requestId, closed);
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
            using var dbContext = new TDSNewContext();

            var deleteAfterDays = SettingsManager.ServerSettings.DeleteRequestsDaysAfterClose;
            var requests = await dbContext.SupportRequests.Where(r => r.CloseTime != null && r.CloseTime.Value.AddDays(deleteAfterDays) < DateTime.UtcNow).ToListAsync();
            if (requests.Any())
            {
                dbContext.SupportRequests.RemoveRange();
                await dbContext.SaveChangesAsync();
            }
        }

        private class SupportRequestsListData
        {
            public int ID { get; set; }
            public string PlayerName { get; set; } = string.Empty;
            public string CreateTime { get; set; } = string.Empty;
            public ESupportType Type { get; set; }
            public string Title { get; set; } = string.Empty;
            public bool Closed { get; set; }

            [JsonIgnore]
            public DateTime CreateTimeDate { get; set; }
        }

        private class SupportRequestData
        {
            public int ID { get; set; }
            public ESupportType Type { get; set; }
            public string Title { get; set; } = string.Empty;
            public IEnumerable<SupportRequestMessageData> Messages { get; set; } = new List<SupportRequestMessageData>();
            public int AtleastAdminLevel { get; set; }
            public bool Closed { get; set; }

            [JsonIgnore]
            public DateTime CreateTimeDate { get; set; }
            [JsonIgnore]
            public int AuthorId { get; set; }
        }

        private class SupportRequestMessageData
        {
            public string Author { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string CreateTime { get; set; } = string.Empty;

            [JsonIgnore]
            public DateTime CreateTimeDate { get; set; }
        }
    }
}
