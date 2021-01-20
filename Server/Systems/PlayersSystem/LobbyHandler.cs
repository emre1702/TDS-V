using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Sync;
using TDS.Shared.Data.Enums;

namespace TDS.Server.PlayersSystem
{
    public class LobbyHandler : IPlayerLobbyHandler
    {
        public IBaseLobby? Current { get; private set; }
        public IBaseLobby? Previous { get; private set; }
        public PlayerLobbyStats? LobbyStats { get; private set; }

        private readonly DataSyncHandler _dataSyncHandler;

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public LobbyHandler(DataSyncHandler dataSyncHandler)
        {
            _dataSyncHandler = dataSyncHandler;
        }

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public async Task SetPlayerLobbyStats(PlayerLobbyStats? playerLobbyStats)
        {
            if (LobbyStats is { })
                await _player.DatabaseHandler.Database.ExecuteForDB(dbContext => dbContext.Entry(LobbyStats).State = EntityState.Detached)
                    .ConfigureAwait(false);

            LobbyStats = playerLobbyStats;

            if (playerLobbyStats != null)
                await _player.DatabaseHandler.Database.ExecuteForDB(dbContext => dbContext.Attach(LobbyStats))
                    .ConfigureAwait(false);
        }

        public async void SetLobby(IBaseLobby? lobby)
        {
            try
            {
                if (lobby == Current)
                    return;
                if (LobbyStats is { })
                {
                    var lobbyStats = LobbyStats;
                    await _player.DatabaseHandler.Database.ExecuteForDB(dbContext => dbContext.Entry(lobbyStats).State = EntityState.Detached).ConfigureAwait(false);
                }

                if (Current is { })
                    Previous = Current;
                Current = lobby;
                SyncLobbyOwnerInfo();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void SyncLobbyOwnerInfo()
        {
            NAPI.Task.RunSafe(() => _dataSyncHandler.SetData(_player, PlayerDataKey.IsLobbyOwner, DataSyncMode.Player, Current?.Players.IsLobbyOwner(_player) == true));
        }
    }
}