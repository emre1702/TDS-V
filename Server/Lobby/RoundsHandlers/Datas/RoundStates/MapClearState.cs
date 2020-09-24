using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class MapClearState : RoundState
    {
        public override int Duration => 1 * 1000;

        public MapClearState(RoundFightLobby lobby) : base(lobby)
        {
        }

        public override void SetCurrent()
        {
            Lobby.Rounds.MapClear();
        }
    }
}
