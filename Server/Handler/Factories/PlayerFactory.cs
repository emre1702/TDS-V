using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Handler.Entities.GTA.GTAPlayer;

namespace TDS_Server.Handler.Factories
{
    public class PlayerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PlayerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.Players.CreateEntity = CreatePlayer;
        }

        private Player CreatePlayer(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSPlayer>(_serviceProvider, netHandle);
    }
}
