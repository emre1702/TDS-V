using System;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Commands;
using TDS_Server.Handler.Maps;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Events
{
    public class RemoteEventsHandler
    {
        private readonly ChatHandler _chatHandler;
        private readonly LoginHandler _loginHandler;
        private readonly MapFavouritesHandler _mapFavouritesHandler;
        private readonly CommandsHandler _commandsHandler;
        private readonly RegisterHandler _registerHandler;
        private readonly ScoreboardHandler _scoreboardHandler;

        public RemoteEventsHandler(ChatHandler chatHandler, LoginHandler loginHandler, MapFavouritesHandler mapFavouritesHandler, CommandsHandler commandsHandler,
            RegisterHandler registerHandler, ScoreboardHandler scoreboardHandler)
            => (_chatHandler, _loginHandler, _mapFavouritesHandler, _commandsHandler, _registerHandler, _scoreboardHandler) 
            = (chatHandler, loginHandler, mapFavouritesHandler, commandsHandler, registerHandler, scoreboardHandler);

        public void LobbyChatMessage(ITDSPlayer player, string message, int chatTypeNumber)
        {
            _chatHandler.SendLobbyMessage(player, message, chatTypeNumber);
        }

        public void TryLogin(ITDSPlayer player, string username, string password)
        {
            _loginHandler.TryLogin(player, username, password);
        }

        public void TryRegister(ITDSPlayer player, string username, string password, string email)
        {
            _registerHandler.TryRegister(player, username, password, email);
        }

        public void ToggleMapFavouriteState(ITDSPlayer tdsPlayer, int mapId, bool isFavorite)
        {
            _mapFavouritesHandler.ToggleMapFavouriteState(tdsPlayer, mapId, isFavorite);
        }

        public void UseCommand(ITDSPlayer tdsPlayer, string msg)
        {
            _commandsHandler.UseCommand(tdsPlayer, msg);
        }

        public void OnLanguageChange(ITDSPlayer player, Language language)
        {
            player.LanguageEnum = language;
        }

        public void OnRequestPlayersForScoreboard(ITDSPlayer tdsPlayer)
        {
            _scoreboardHandler.OnRequestPlayersForScoreboard(tdsPlayer);
        }
    }
}
