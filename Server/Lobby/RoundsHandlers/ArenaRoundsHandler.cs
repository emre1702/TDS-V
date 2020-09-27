using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers
{
    public class ArenaRoundsHandler : RoundFightLobbyRoundsHandler
    {
        protected new Arena Lobby => (Arena)base.Lobby;

        public ArenaRoundsHandler(Arena lobby) : base(lobby)
        {
        }

        public override ValueTask<ITeam?> GetTimesUpWinnerTeam()
            => new ValueTask<ITeam?>(Lobby.Teams.GetTeamWithHighestHp());
    }
}
