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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    ExecuteForDB(dbContext => dbContext.Entry(_lobbyStats).State = EntityState.Detached);

                _lobbyStats = value;
                if (value != null)
                    ExecuteForDB(dbContext => dbContext.Attach(_lobbyStats));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }
        public RoundStatsDto? CurrentRoundStats { get; set; }
        public bool IsLobbyOwner => Lobby?.IsPlayerLobbyOwner(this) ?? false;


    }
}
