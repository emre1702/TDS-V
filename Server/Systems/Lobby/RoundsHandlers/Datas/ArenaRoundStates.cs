using System;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.RoundEndReasons;
using TDS.Server.Handler;
using TDS.Server.LobbySystem.RoundsHandlers.Datas.RoundStates;

namespace TDS.Server.LobbySystem.RoundsHandlers.Datas
{
    public class ArenaRoundStates : RoundFightLobbyRoundStates
    {
        private readonly IArena _arena;

        public ArenaRoundStates(IArena arena, IRoundFightLobbyEventsHandler events) : base(arena, events)
            => _arena = arena;

        public override async void SetNext()
        {
            try
            {
                if (Next.Value is RoundEndState)
                {
                    var winnerTeam = await _arena.Rounds.GetTimesUpWinnerTeam().ConfigureAwait(false);
                    CurrentRoundEndReason = new TimeRoundEndReason(winnerTeam);
                }

                base.SetNext();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }
    }
}
