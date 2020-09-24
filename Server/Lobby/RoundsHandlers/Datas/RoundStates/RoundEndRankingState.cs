using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class RoundEndRankingState : RoundState
    {
        public override int Duration => 10 * 1000;

        public RoundEndRankingState(RoundFightLobby lobby) : base(lobby)
        {
        }

        public override void SetCurrent()
        {
            Lobby.Rounds.RoundEndRanking();
        }
    }
}
