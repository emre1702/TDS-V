using Microsoft.EntityFrameworkCore;
using TDS_Server.Dto;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Core.Instance.PlayerInstance
{
    partial class TDSPlayer
    {
        private PlayerLobbyStats? _currentLobbyStats;

        public LobbyInstances.Lobby? CurrentLobby { get; set; }
        public LobbyInstances.Lobby? PreviousLobby { get; set; }
        public PlayerLobbyStats? CurrentLobbyStats
        {
            get => _currentLobbyStats;
            set
            {
                if (_currentLobbyStats != null)
                    DbContext.Entry(_currentLobbyStats).State = EntityState.Detached;
                _currentLobbyStats = value;
                if (value != null)
                    DbContext.Attach(_currentLobbyStats);
            }
        }
        public RoundStatsDto? CurrentRoundStats { get; set; }
        public bool IsLobbyOwner => CurrentLobby?.IsPlayerLobbyOwner(this) ?? false;

        
    }
}
