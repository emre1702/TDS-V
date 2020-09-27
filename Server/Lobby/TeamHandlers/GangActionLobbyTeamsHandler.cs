using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class GangActionLobbyTeamsHandler : RoundFightLobbyTeamsHandler
    {
        public GangActionLobbyTeamsHandler(GangActionLobby lobby, IRoundFightLobbyEventsHandler events, LangHelper langHelper)
            : base(lobby, events, langHelper)
        {
        }
    }
}
