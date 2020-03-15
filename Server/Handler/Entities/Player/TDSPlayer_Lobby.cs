using Microsoft.EntityFrameworkCore;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        private PlayerLobbyStats? _lobbyStats;

        public ILobby? Lobby { get; set; }
        public ILobby? PreviousLobby { get; set; }
        public PlayerLobbyStats? LobbyStats
        {
            get => _lobbyStats;
            set
            {
                if (_lobbyStats != null)
                    DbContext.Entry(_lobbyStats).State = EntityState.Detached;
                _lobbyStats = value;
                if (value != null)
                    DbContext.Attach(_lobbyStats);
            }
        }
        public RoundStatsDto? CurrentRoundStats { get; set; }
        public bool IsLobbyOwner => Lobby?.IsPlayerLobbyOwner(this) ?? false;


    }
}
