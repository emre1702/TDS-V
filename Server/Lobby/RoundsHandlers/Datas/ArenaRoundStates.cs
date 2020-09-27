using System.Threading.Tasks;
using TDS_Server.Data.RoundEndReasons;
using TDS_Server.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas
{
    public class ArenaRoundStates : RoundFightLobbyRoundStates
    {
        private readonly Arena _arena;

        public ArenaRoundStates(Arena arena) : base(arena)
            => _arena = arena;

        public override async ValueTask SetNext()
        {
            if (Next.Value is RoundEndState)
            {
                var winnerTeam = await _arena.Rounds.GetTimesUpWinnerTeam();
                CurrentRoundEndReason = new TimeRoundEndReason(winnerTeam);
            }

            await base.SetNext();
        }
    }
}
