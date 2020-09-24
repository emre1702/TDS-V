using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class CountdownState : RoundState
    {
        private int _duration = 5 * 1000;

        public override int Duration => _duration;

        public CountdownState(RoundFightLobby lobby) : base(lobby)
        {
        }

        public override void LoadSettings()
        {
            _duration = Lobby.Entity.LobbyRoundSettings.CountdownTime;
        }

        public override void SetCurrent()
        {
            Lobby.Rounds.Countdown();
        }
    }
}
