using GTANetworkAPI;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class InRoundState : RoundState
    {
        private int _duration = 4 * 60 * 1000;

        public override int Duration => _duration;

        public InRoundState(RoundFightLobby lobby) : base(lobby)
        {
        }

        public override void LoadSettings()
        {
            _duration = Lobby.Entity.LobbyRoundSettings.RoundTime;
        }

        public override void SetCurrent()
        {
            Lobby.Rounds.InRound();
        }
    }
}
