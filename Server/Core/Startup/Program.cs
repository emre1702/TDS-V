using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Interfaces.ModAPI;

namespace TDS_Server.Core.Startup
{
    public class Program
    {
        private static ServiceProvider _serviceProvider;

        public Program(IModAPI modAPI)
        {
            var serviceCollection = Services.InitServiceCollection();

            serviceCollection
                .AddSingleton(modAPI);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

    }
}
