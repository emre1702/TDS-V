using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Handler.Entities.GTA.Pickups;

namespace TDS_Server.Handler.Factories
{
    public class PickupFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PickupFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.Pickups.CreateEntity = CreatePickup;
        }

        private Pickup CreatePickup(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSPickup>(_serviceProvider, netHandle);
    }
}
