using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Database.Entity;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler
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
                if (!_chatInfosJsonCache.TryGetValue(player.LanguageHandler.EnumValue, out string? json))
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
                _chatInfosJsonCache[entry.Key] = Serializer.ToBrowser(entry.Value);
            }
        }

    }
}
