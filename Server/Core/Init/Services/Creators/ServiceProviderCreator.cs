using Microsoft.Extensions.DependencyInjection;
using TDS.Server.Data.Utility;

namespace TDS.Server.Core.Init.Services.Creators
{
    internal static class ServiceProviderCreator
    {
        public static CustomServiceProvider Create()
        {
            var serviceCollection = new ServiceCollection()
                .WithDatabase()
                .WithHandlers()
                .WithLobby()
                .WithGamemode()
                .WithTeam();

            var provider = new CustomServiceProvider(serviceCollection);
            return provider;
        }
    }
}
