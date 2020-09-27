using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class GangLobbyTeamsHandler : BaseLobbyTeamsHandler
    {
        public GangLobbyTeamsHandler(GangLobby lobby, IBaseLobbyEventsHandler events) : base(lobby, events)
        {
        }

        protected override void Events_PlayerJoined(ITDSPlayer player, int _)
        {
            SetPlayerTeam(player, player.Gang.GangLobbyTeam);
        }
    }
}
