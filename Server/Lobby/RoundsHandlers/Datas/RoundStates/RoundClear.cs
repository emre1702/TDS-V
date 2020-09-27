using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class RoundClear : RoundState
    {
        public override int Duration => 1 * 1000;

        public RoundClear(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override ValueTask SetCurrent()
            => Lobby.Events.TriggerRoundClear();
    }
}
