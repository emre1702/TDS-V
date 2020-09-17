using Microsoft.Extensions.DependencyInjection;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class ServiceProviderCreator
    {
        public static CustomServiceProvider Create()
        {
            var serviceCollection = new ServiceCollection()
                .WithDatabase()
                .WithHandlers();

            var provider = new CustomServiceProvider(serviceCollection);
            return provider;
        }
    }
}
