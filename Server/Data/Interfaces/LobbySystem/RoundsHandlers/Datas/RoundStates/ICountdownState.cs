using System.Threading.Tasks;

namespace TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public interface ICountdownState
    {
        int Duration { get; }

        void LoadSettings();
        ValueTask SetCurrent();
    }
}