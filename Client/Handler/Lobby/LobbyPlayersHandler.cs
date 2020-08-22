using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyPlayersHandler
    {
        #region Public Fields

        public List<IPlayer> PlayersSameLobby = new List<IPlayer>();

        #endregion Public Fields

        #region Private Fields

        private readonly BrowserHandler _browserHandler;

        #endregion Private Fields

        #region Public Constructors

        public LobbyPlayersHandler(BrowserHandler browserHandler, EventsHandler eventsHandler)
        {
            _browserHandler = browserHandler;

            eventsHandler.PlayerJoinedSameLobby += Load;
            eventsHandler.PlayerLeftSameLobby += Remove;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(List<IPlayer> players)
        {
            PlayersSameLobby = players;
            _browserHandler.Angular.LoadNamesForChat(players);
        }

        public void Load(IPlayer player)
        {
            PlayersSameLobby.Add(player);
        }

        public void Remove(IPlayer player, string name)
        {
            PlayersSameLobby.Remove(player);
            _browserHandler.Angular.RemoveNameForChat(name);
        }

        #endregion Public Methods
    }
}
