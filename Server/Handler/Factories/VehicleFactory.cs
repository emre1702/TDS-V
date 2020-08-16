using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler.Factories
{
    public class VehicleFactory : IEntityFactory<IVehicle>
    {
        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;

        public VehicleFactory(IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
            => _entitiesByInterfaceCreator = entitiesByInterfaceCreator;


        public IVehicle Create(IntPtr entityPointer, ushort id)
            => _entitiesByInterfaceCreator.Create<ITDSVehicle>(entityPointer, id);
    }
}
