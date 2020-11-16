using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class NewMapChooseState : RoundState
    {
        public override int Duration => 500;

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
