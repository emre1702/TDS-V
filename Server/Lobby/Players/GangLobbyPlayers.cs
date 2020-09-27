using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.Players
{
    public class GangLobbyPlayers : BaseLobbyPlayers
    {
        public GangLobbyPlayers(GangLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }
    }
}
