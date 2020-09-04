using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Handler.Entities.GTA.Objects;

namespace TDS_Server.Handler.Factories
{
    public class ObjectFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ObjectFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.Objects.CreateEntity = CreateObject;
        }

        private GTANetworkAPI.Object CreateObject(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSObject>(_serviceProvider, netHandle);
    }
}
