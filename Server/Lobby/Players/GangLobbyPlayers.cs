using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Players
{
    public class GangLobbyPlayers : BaseLobbyPlayers
    {
        public GangLobbyPlayers(GangLobby lobby, IBaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams, BaseLobbyBansHandler bans)
            : base(lobby, events, teams, bans)
        {
        }
    }
}
