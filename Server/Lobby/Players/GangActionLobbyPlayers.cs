using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.Players
{
    public class GangActionLobbyPlayers : RoundFightLobbyPlayers
    {
        public GangActionLobbyPlayers(GangActionLobby lobby, IRoundFightLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        public override Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            return base.AddPlayer(player, teamIndex);
        }
    }
}
