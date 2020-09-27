using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

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

        public override void SetLobby(IBaseLobby lobby)
        {
            LobbyNew = lobby;
            NAPI.Task.Run(() => _dataSyncHandler.SetData(this, PlayerDataKey.IsLobbyOwner, DataSyncMode.Player, lobby.Players.IsLobbyOwner(this)));
        }
    }
}
