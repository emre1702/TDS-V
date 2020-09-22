using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Players
{
    public class ArenaPlayers : FightLobbyPlayers
    {
        public ArenaPlayers(Arena arena, IBaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams, BaseLobbyBansHandler bans)
            : base(arena, events, teams, bans)
        {
        }
    }
}
