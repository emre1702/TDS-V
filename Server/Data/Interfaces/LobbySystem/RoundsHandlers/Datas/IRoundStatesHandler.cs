using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;

namespace TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas
{
    public interface IRoundStatesHandler
    {
        IRoundEndReason CurrentRoundEndReason { get; }
        IRoundState CurrentState { get; }
        bool Started { get; }
        int TimeToNextStateMs { get; }
        int TimeInStateMs { get; }

        void EndRound(IRoundEndReason roundEndReason);

        void SetNext();

        void StartRound();

        void StopRound();

        Task<IDisposable> GetContext([CallerMemberName] string calledFrom = "");

        bool IsCurrentStateBeforeRoundEnd();

        Task<bool> IsCurrentStateBeforeRoundEndBlocked();

        bool IsCurrentStateAfterRound();
    }
}
