using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        public ILobby? Lobby { get; set; }
        public ILobby? PreviousLobby { get; set; }
        public PlayerLobbyStats? LobbyStats { get; private set; }
        public RoundStatsDto? CurrentRoundStats { get; set; }
        public bool IsLobbyOwner => Lobby?.IsPlayerLobbyOwner(this) ?? false;


        public async Task SetPlayerLobbyStats(PlayerLobbyStats? playerLobbyStats)
        {
            if (LobbyStats != null)
                await ExecuteForDB(dbContext => dbContext.Entry(LobbyStats).State = EntityState.Detached);

            LobbyStats = playerLobbyStats;

            if (playerLobbyStats != null)
                await ExecuteForDB(dbContext => dbContext.Attach(LobbyStats));
        }
    }
}
