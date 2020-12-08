using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class RoundEndStatsState : RoundEndState
    {
        public override int Duration => 500;

        public RoundEndStatsState(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override ValueTask SetCurrent()
            => Lobby.Events.TriggerRoundEndStats();
    }
}
