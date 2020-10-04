using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces.GamemodesSystem;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.GamemodesSystem.Gamemodes;
using TDS_Shared.Data.Enums;

namespace TDS_Server.GamemodesSystem
{
    public class GamemodesProvider : IGamemodesProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public GamemodesProvider(TDSDbContext dbContext, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            SniperGamemode.Init(dbContext);
        }

        public TGamemode Get<TGamemode>(IRoundFightLobby lobby, MapDto map) where TGamemode : IBaseGamemode
        {
            var gamemode = _serviceProvider.GetService<TGamemode>();
            if (gamemode is null)
                throw new ArgumentException($"LobbiesProvider doesn't create lobby with Lobby entity for type '{typeof(TGamemode).FullName}'.");

            gamemode.Init(lobby, map);

            return gamemode;
        }

        public IBaseGamemode Get(IRoundFightLobby lobby, MapDto map)
        {
            IBaseGamemode gamemode = map.BrowserSyncedData.Type switch
            {
                MapType.Normal => _serviceProvider.GetRequiredService<IDeathmatchGamemode>(),
                MapType.Bomb => _serviceProvider.GetRequiredService<IBombGamemode>(),
                MapType.Sniper => _serviceProvider.GetRequiredService<ISniperGamemode>(),
                MapType.Gangwar => _serviceProvider.GetRequiredService<IGangwarGamemode>(),
                //MapType.ArmsRace => _serviceProvider.GetRequiredService<IArmsRace>(),

                _ => throw new ArgumentException($"LobbiesProvider doesn't create lobby with map for map type '{map.BrowserSyncedData.Type}'.")
            };
            gamemode.Init(lobby, map);

            return gamemode;
        }
    }
}
