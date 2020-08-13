using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Public Properties

        public RoundStatsDto? CurrentRoundStats { get; set; }
        public bool IsLobbyOwner => Lobby?.IsPlayerLobbyOwner(this) ?? false;
        public ILobby? Lobby { get; set; }
        public PlayerLobbyStats? LobbyStats { get; private set; }
        public ILobby? PreviousLobby { get; set; }

        #endregion Public Properties

        #region Public Methods

        public async Task SetPlayerLobbyStats(PlayerLobbyStats? playerLobbyStats)
        {
            if (LobbyStats != null)
                await ExecuteForDB(dbContext => dbContext.Entry(LobbyStats).State = EntityState.Detached);

            LobbyStats = playerLobbyStats;

            if (playerLobbyStats != null)
                await ExecuteForDB(dbContext => dbContext.Attach(LobbyStats));
        }

        #endregion Public Methods
    }
}
