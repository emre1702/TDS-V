using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public abstract class RoundState
    {
        public abstract int Duration { get; }

        protected readonly IRoundFightLobby Lobby;

        public RoundState(IRoundFightLobby lobby)
        {
            Lobby = lobby;
            LoadSettings();
        }

        public abstract ValueTask SetCurrent();

        public virtual void LoadSettings()
        {
        }
    }
}
