using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Maps;

namespace TDS_Server.Handler.Events
{
    public class RemoteEventsHandler
    {
        private readonly ChatHandler _chatHandler;
        private readonly LoginHandler _loginHandler;
        private readonly MapFavouritesHandler _mapFavouritesHandler;

        public RemoteEventsHandler(ChatHandler chatHandler, LoginHandler loginHandler, MapFavouritesHandler mapFavouritesHandler)
            => (_chatHandler, _loginHandler, _mapFavouritesHandler) = (chatHandler, loginHandler, mapFavouritesHandler);

        public void LobbyChatMessage(ITDSPlayer player, string message, int chatTypeNumber)
        {
            _chatHandler.SendLobbyMessage(player, message, chatTypeNumber);
        }

        public void TryLogin(ITDSPlayer player, string username, string password)
        {
            _loginHandler.TryLogin(player, username, password);
        }

        public void ToggleMapFavouriteState(ITDSPlayer tdsPlayer, int mapId, bool isFavorite)
        {
            _mapFavouritesHandler.ToggleMapFavouriteState(tdsPlayer, mapId, isFavorite);
        }
    }
}
