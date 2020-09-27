using TDS_Server.LobbySystem.EventsHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class GangActionLobbyTeamsHandler : RoundFightLobbyTeamsHandler
    {
        public GangActionLobbyTeamsHandler(LobbyDb entity, BaseLobbyEventsHandler events) : base(entity, events)
        {
        }
    }
}
