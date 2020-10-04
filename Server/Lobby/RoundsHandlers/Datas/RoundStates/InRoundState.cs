using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class InRoundState : RoundState, IInRoundState
    {
        private int _duration = 4 * 60 * 1000;

        public override int Duration => _duration;

        public InRoundState(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override void LoadSettings()
        {
            _duration = Lobby.Entity.LobbyRoundSettings.RoundTime * 1000;
        }

        public override ValueTask SetCurrent()
        {
            Lobby.Events.TriggerInRound();
            return default;
        }
    }
}
