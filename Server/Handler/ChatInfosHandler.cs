using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler
{
    public class ChatInfosHandler
    {
        #region Private Fields

        private readonly Dictionary<Language, string> _chatInfosJsonCache = new Dictionary<Language, string>();

        #endregion Private Fields

        #region Public Constructors

        public ChatInfosHandler(TDSDbContext dbContext, Serializer serializer, EventsHandler eventsHandler)
        {
            LoadChatInfos(dbContext, serializer);

            eventsHandler.PlayerLoggedIn += SendChatInfos;
        }

        #endregion Public Constructors

        #region Public Methods

        public void SendChatInfos(ITDSPlayer player)
        {
            if (!_chatInfosJsonCache.TryGetValue(player.LanguageEnum, out string? json))
                return;

            player.SendBrowserEvent(ToBrowserEvent.LoadChatInfos, json);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadChatInfos(TDSDbContext dbContext, Serializer serializer)
        {
            var data = dbContext.ChatInfos
                .GroupBy(c => c.Language)
                .ToDictionary(c => c.Key, c => c.Select(e => e.Message).ToList());

            foreach (var entry in data)
            {
                _chatInfosJsonCache[entry.Key] = serializer.ToBrowser(entry.Value);
            }
        }

        #endregion Private Methods
    }
}
