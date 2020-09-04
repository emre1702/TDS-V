using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Handler.Entities.GTA.ColShapes;

namespace TDS_Server.Handler.Factories
{
    public class ColShapeFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ColShapeFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.Colshapes.CreateEntity = CreateColShape;
        }

        private ColShape CreateColShape(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSColShape>(_serviceProvider, netHandle);
    }
}
