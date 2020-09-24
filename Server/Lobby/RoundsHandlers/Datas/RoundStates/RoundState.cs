using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public abstract class RoundState
    {
        public abstract int Duration { get; }

        protected readonly RoundFightLobby Lobby;

        public RoundState(RoundFightLobby lobby)
        {
            Lobby = lobby;
            LoadSettings();
        }

        public abstract void SetCurrent();

        public virtual void LoadSettings()
        {
        }
    }
}
