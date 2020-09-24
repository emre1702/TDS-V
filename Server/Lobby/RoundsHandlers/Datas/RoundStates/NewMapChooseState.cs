using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class NewMapChooseState : RoundState
    {
        public override int Duration => 4 * 1000;

        public NewMapChooseState(RoundFightLobby lobby) : base(lobby)
        {
        }

        public override void SetCurrent()
        {
            Lobby.Rounds.NewMapChoose();
        }
    }
}
