using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;

namespace TDS.Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public abstract class RoundState : IRoundState
    {
        public abstract int Duration { get; }

        protected IRoundFightLobby Lobby { get; }

        protected RoundState(IRoundFightLobby lobby)
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
