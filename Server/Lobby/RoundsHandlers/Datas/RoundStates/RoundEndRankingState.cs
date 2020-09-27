using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class RoundEndRankingState : RoundState
    {
        public override int Duration => 10 * 1000;

        public RoundEndRankingState(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override ValueTask SetCurrent()
            => Lobby.Events.TriggerRoundEndRanking();
    }
}
