using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.LobbySystem.EventsHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class GangLobbyTeamsHandler : BaseLobbyTeamsHandler
    {
        public GangLobbyTeamsHandler(LobbyDb entity, BaseLobbyEventsHandler events) : base(entity, events)
        {
        }

        protected override void Events_PlayerJoined(ITDSPlayer player, int _)
        {
            SetPlayerTeam(player, player.Gang.GangLobbyTeam);
        }
    }
}
