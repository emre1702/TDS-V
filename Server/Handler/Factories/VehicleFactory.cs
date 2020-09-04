using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Handler.Entities.GTA.Vehicles;

namespace TDS_Server.Handler.Factories
{
    public class VehicleFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public VehicleFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.Vehicles.CreateEntity = CreateVehicle;
        }

        private Vehicle CreateVehicle(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSVehicle>(_serviceProvider, netHandle);
    }
}
