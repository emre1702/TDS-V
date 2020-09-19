using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        public override RoundStatsDto? CurrentRoundStats { get; set; }
        public override bool IsLobbyOwner => Lobby?.IsPlayerLobbyOwner(this) ?? false;
        public override PlayerLobbyStats? LobbyStats { get; set; }
        public override ILobby? PreviousLobby { get; set; }

        public override async Task SetPlayerLobbyStats(PlayerLobbyStats? playerLobbyStats)
        {
            if (LobbyStats != null)
                await Database.ExecuteForDB(dbContext => dbContext.Entry(LobbyStats).State = EntityState.Detached);

            LobbyStats = playerLobbyStats;

            if (playerLobbyStats != null)
                await Database.ExecuteForDB(dbContext => dbContext.Attach(LobbyStats));
        }
    }
}
