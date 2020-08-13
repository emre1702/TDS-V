using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TDS_Server.Handler.Factories
{
    public class VehicleFactory : IEntityFactory<IVehicle>
    {
        private readonly IServiceProvider _serviceProvider;

        public VehicleFactory(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public IVehicle Create(IntPtr entityPointer, ushort id)
            => ActivatorUtilities.CreateInstance<IVehicle>(_serviceProvider, entityPointer, id);
    }
}
