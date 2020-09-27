using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas
{
    public interface IRoundStatesHandler
    {
        public IRoundEndReason CurrentRoundEndReason { get; }

        void EndRound(IRoundEndReason roundEndReason);

        ValueTask SetNext();

        void Start();

        void Stop();
    }
}
