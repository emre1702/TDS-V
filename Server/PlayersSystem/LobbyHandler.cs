﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;

namespace TDS_Server.PlayersSystem
{
    public class LobbyHandler : IPlayerLobbyHandler
    {
        public IBaseLobby? Current { get; private set; }
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
                await _player.DatabaseHandler.Database.ExecuteForDB(dbContext => dbContext.Entry(LobbyStats).State = EntityState.Detached);

            LobbyStats = playerLobbyStats;

            if (playerLobbyStats != null)
                await _player.DatabaseHandler.Database.ExecuteForDB(dbContext => dbContext.Attach(LobbyStats));
        }

        public async void SetLobby(IBaseLobby lobby)
        {
            if (LobbyStats is { })
                await _player.DatabaseHandler.Database.ExecuteForDB(dbContext => dbContext.Entry(LobbyStats).State = EntityState.Detached);

            Current = lobby;
            NAPI.Task.Run(() => _dataSyncHandler.SetData(_player, PlayerDataKey.IsLobbyOwner, DataSyncMode.Player, lobby.Players.IsLobbyOwner(_player)));
        }
    }
}
