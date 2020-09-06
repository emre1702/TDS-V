using System.Collections.Generic;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyPlayersHandler
    {
        #region Public Fields

        public List<ITDSPlayer> PlayersSameLobby = new List<ITDSPlayer>();

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

        public void Load(List<ITDSPlayer> players)
        {
            PlayersSameLobby = players;
            _browserHandler.Angular.LoadNamesForChat(players);
        }

        public void Load(ITDSPlayer player)
        {
            PlayersSameLobby.Add(player);
            _browserHandler.Angular.AddNameForChat(player.Name);
        }

        public void Remove(ITDSPlayer player, string name)
        {
            PlayersSameLobby.Remove(player);
            _browserHandler.Angular.RemoveNameForChat(name);
        }

        #endregion Public Methods
    }
}
