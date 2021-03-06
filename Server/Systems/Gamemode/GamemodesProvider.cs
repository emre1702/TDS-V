﻿using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Data.Interfaces.GamemodesSystem;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models.Map;
using TDS.Server.Database.Entity;
using TDS.Server.GamemodesSystem.Gamemodes;
using TDS.Shared.Data.Enums;

namespace TDS.Server.GamemodesSystem
{
    public class GamemodesProvider : IGamemodesProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public GamemodesProvider(TDSDbContext dbContext, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            SniperGamemode.Init(dbContext);
        }

        public TGamemode Create<TGamemode>(IRoundFightLobby lobby, MapDto map) where TGamemode : IBaseGamemode
        {
            var gamemode = _serviceProvider.GetService<TGamemode>();
            if (gamemode is null)
                throw new ArgumentException($"LobbiesProvider doesn't create lobby with Lobby entity for type '{typeof(TGamemode).FullName}'.");

            gamemode.Init(lobby, map);

            return gamemode;
        }

        public IBaseGamemode Create(IRoundFightLobby lobby, MapDto map)
        {
            IBaseGamemode gamemode = map.BrowserSyncedData.Type switch
            {
                MapType.Normal => _serviceProvider.GetRequiredService<IDeathmatchGamemode>(),
                MapType.Bomb => _serviceProvider.GetRequiredService<IBombGamemode>(),
                MapType.Sniper => _serviceProvider.GetRequiredService<ISniperGamemode>(),
                MapType.Gangwar => _serviceProvider.GetRequiredService<IGangwarGamemode>(),
                MapType.ArmsRace => _serviceProvider.GetRequiredService<IArmsRaceGamemode>(),

                _ => throw new ArgumentException($"LobbiesProvider doesn't create lobby with map for map type '{map.BrowserSyncedData.Type}'.")
            };
            gamemode.Init(lobby, map);

            return gamemode;
        }
    }
}
