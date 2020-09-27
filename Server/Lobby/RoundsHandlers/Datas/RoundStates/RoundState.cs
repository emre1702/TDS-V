using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public abstract class RoundState : IRoundState
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
