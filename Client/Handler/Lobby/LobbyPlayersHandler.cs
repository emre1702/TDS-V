using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Browser;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyPlayersHandler
    {
        public List<IPlayer> PlayersSameLobby = new List<IPlayer>();

        private readonly BrowserHandler _browserHandler;

        public LobbyPlayersHandler(BrowserHandler browserHandler)
        {
            _browserHandler = browserHandler;
        }

        public void Load(List<IPlayer> players)
        {
            PlayersSameLobby = players;
            _browserHandler.Angular.LoadNamesForChat(players);
        }

        public void Load(IPlayer player)
        {
            PlayersSameLobby.Add(player);
            _browserHandler.Angular.AddNameForChat(player.Name);
        }

        public void Remove(IPlayer player, string name)
        {
            PlayersSameLobby.Remove(player);
            _browserHandler.Angular.RemoveNameForChat(name);
        }
    }
}
