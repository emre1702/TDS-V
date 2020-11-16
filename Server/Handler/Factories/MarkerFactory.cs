using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Handler.Entities.GTA.Markers;

namespace TDS.Server.Handler.Factories
{
    public class MarkerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MarkerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.Markers.CreateEntity = CreateMarker;
        }

        private Marker CreateMarker(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSMarker>(_serviceProvider, netHandle);
    }
}
