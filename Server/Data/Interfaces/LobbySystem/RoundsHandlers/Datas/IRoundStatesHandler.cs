using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas
{
    public interface IRoundStatesHandler
    {
        IRoundEndReason CurrentRoundEndReason { get; }
        IRoundState CurrentState { get; }
        int TimeToNextStateMs { get; }
        int TimeInStateMs { get; }

        void EndRound(IRoundEndReason roundEndReason);

        void SetNext();

        void Start();

        void Stop();

        Task<IDisposable> GetContext();

        bool IsCurrentStateBeforeRoundEnd();

        Task<bool> IsCurrentStateBeforeRoundEndBlocked();
    }
}
