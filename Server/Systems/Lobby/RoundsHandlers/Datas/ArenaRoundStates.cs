using System;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.RoundEndReasons;
using TDS_Server.Handler;
using TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas
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
