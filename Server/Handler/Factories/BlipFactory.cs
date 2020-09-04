using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Handler.Entities.GTA.Blips;

namespace TDS_Server.Handler.Factories
{
    public class BlipFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BlipFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.Blips.CreateEntity = CreateBlip;
        }

        private Blip CreateBlip(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSBlip>(_serviceProvider, netHandle);
    }
}
