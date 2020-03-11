using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Core.Player.Join;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler;
using TDS_Server.Handler.Events.Mod;
using TDS_Server.Handler.Player;

namespace TDS_Server.Core.Startup
{
    internal static class Services
    {
        internal static ServiceProvider InitServiceCollection(IModAPI modAPI)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddSingleton(modAPI)
                .AddSingleton<ConnectedHandler>()
                .AddSingleton<TDSPlayerHandler>()
                .AddSingleton<EventsHandler>()
                .AddSingleton<MappingHandler>()
                .AddSingleton<BansHandler>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
