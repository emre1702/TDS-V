using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Handler;

namespace TDS.Server.PlayersSystem
{
    public class Provider : IPlayerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public Provider(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public ITDSPlayer Create(NetHandle netHandle)
        {
            try
            {
                Console.WriteLine($"Creating ITDSPlayer for: {netHandle.Value}");
                var player = ActivatorUtilities.CreateInstance<TDSPlayer>(_serviceProvider, netHandle);
                if (player is null)
                    Console.WriteLine("player could not be created!! ERROOORR!!");
                else
                    Console.WriteLine($"Created ITDSPlayer for: {netHandle.Value}");
                return player!;
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
                return null!;
            }
        }
    }
}