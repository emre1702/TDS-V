using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class InRoundState : RoundState
    {
        private int _duration = 4 * 60 * 1000;

        public override int Duration => _duration;

        public InRoundState(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override void LoadSettings()
        {
            _duration = Lobby.Entity.LobbyRoundSettings.RoundTime;
        }

        public override ValueTask SetCurrent()
        {
            Lobby.Events.TriggerInRound();
            return default;
        }
    }
}
