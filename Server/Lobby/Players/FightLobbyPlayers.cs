using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Players
{
    public class FightLobbyPlayers : BaseLobbyPlayers
    {
        public FightLobbyPlayers(LobbyDb entity, BaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams) : base(entity, events, teams)
        {
        }
    }
}
