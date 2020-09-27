using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class NewMapChooseState : RoundState
    {
        public override int Duration => 2 * 1000;

        public NewMapChooseState(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override ValueTask SetCurrent()
        {
            Lobby.Events.TriggerNewMapChoose();
            return default;
        }
    }
}
