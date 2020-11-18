using System.Collections.Generic;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;

namespace TDS.Client.Handler.Lobby
{
    public class LobbyPlayersHandler
    {
        public List<ITDSPlayer> PlayersSameLobby = new List<ITDSPlayer>();

        private readonly BrowserHandler _browserHandler;

        public LobbyPlayersHandler(BrowserHandler browserHandler, EventsHandler eventsHandler)
        {
            _browserHandler = browserHandler;

            eventsHandler.PlayerJoinedSameLobby += Load;
            eventsHandler.PlayerLeftSameLobby += Remove;
        }

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
    }
}
