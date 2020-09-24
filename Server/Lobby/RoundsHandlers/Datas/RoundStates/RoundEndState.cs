using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class RoundEndState : RoundState
    {
        public override int Duration => 1 * 1000;

        public RoundEndState(RoundFightLobby lobby) : base(lobby)
        {
        }

        public override void SetCurrent()
        {
            Lobby.Rounds.RoundEnd();
        }
    }
}
