using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class CountdownState : RoundState
    {
        private int _duration = 5 * 1000;

        public override int Duration => _duration;

        public CountdownState(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override void LoadSettings()
        {
            _duration = Lobby.Entity.LobbyRoundSettings.CountdownTime * 1000;
        }

        public override ValueTask SetCurrent()
        {
            Lobby.Events.TriggerCountdown();
            return default;
        }
    }
}
