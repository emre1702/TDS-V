using System;
using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models.Map;
using TDS_Shared.Data.Enums;

namespace TDS_Server.LobbySystem.GamemodesHandlers
{
    internal class GamemodesProvider
    {
        private readonly IRoundFightLobby _lobby;
        private readonly IServiceProvider _serviceProvider;

        public GamemodesProvider(IRoundFightLobby lobby, IServiceProvider serviceProvider)
        {
            _lobby = lobby;
            _serviceProvider = serviceProvider;
        }

        public IGamemode Get(MapDto mapDto)
        {
            IGamemode gamemode = mapDto.BrowserSyncedData.Type switch
            {
                MapType.Normal => _serviceProvider.GetRequiredService<IDeathmatch>(),
                MapType.Bomb => _serviceProvider.GetRequiredService<IBomb>(),
                MapType.Sniper => _serviceProvider.GetRequiredService<ISniper>(),
                MapType.Gangwar => _serviceProvider.GetRequiredService<IGangwar>(),
                MapType.ArmsRace => _serviceProvider.GetRequiredService<IArmsRace>(),
                _ => _serviceProvider.GetRequiredService<IDeathmatch>()
            };
            gamemode.Init(_lobby, mapDto);

            return gamemode;
        }
    }
}
