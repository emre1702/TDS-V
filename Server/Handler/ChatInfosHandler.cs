using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler
{
    public class ChatInfosHandler
    {
        private readonly Dictionary<Language, string> _chatInfosJsonCache = new Dictionary<Language, string>();

        public ChatInfosHandler(TDSDbContext dbContext, EventsHandler eventsHandler)
        {
            LoadChatInfos(dbContext);

            eventsHandler.PlayerLoggedIn += SendChatInfos;
        }

        public void SendChatInfos(ITDSPlayer player)
        {
            lock (_chatInfosJsonCache)
            {
                if (!_chatInfosJsonCache.TryGetValue(player.LanguageHandler.Enum, out string? json))
                    return;

                NAPI.Task.RunSafe(() =>
                    player.TriggerBrowserEvent(ToBrowserEvent.LoadChatInfos, json));
            }
            
        }

        private void LoadChatInfos(TDSDbContext dbContext)
        {
            var data = dbContext.ChatInfos
                .ToList()
                .GroupBy(c => c.Language)
                .ToDictionary(c => c.Key, c => c.Select(e => e.Message).ToList());

            foreach (var entry in data)
            {
                _chatInfosJsonCache[entry.Key] = Serializer.ToBrowser(entry.Value).Replace("\\", "\\\\");
            }
        }

    }
}
