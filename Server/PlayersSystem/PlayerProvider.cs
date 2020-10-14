using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.PlayersSystem;

namespace TDS_Server.PlayersSystem
{
    public class Provider : IPlayerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public Provider(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public ITDSPlayer Create(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSPlayer>(_serviceProvider, netHandle);
    }
}
