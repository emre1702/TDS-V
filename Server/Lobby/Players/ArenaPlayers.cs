using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Players
{
    public class ArenaPlayers : FightLobbyPlayers
    {
        public ArenaPlayers(LobbyDb entity, BaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams) : base(entity, events, teams)
        {
        }
    }
}
