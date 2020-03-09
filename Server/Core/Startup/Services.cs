using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Core.Player.Join;

namespace TDS_Server.Core.Startup
{
    internal class Services
    {
        internal static IServiceCollection InitServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            return serviceCollection
                .AddSingleton<ConnectedHandler>();
        }
    }
}
