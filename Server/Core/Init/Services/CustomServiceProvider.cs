using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace TDS_Server.Core.Init.Services
{
    public class CustomServiceProvider : IServiceProvider
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly ServiceProvider _serviceProvider;

        public CustomServiceProvider(IServiceCollection collection)
        {
            _serviceCollection = collection;
            _serviceProvider = collection.BuildServiceProvider();
        }

        public object GetService(Type serviceType)
            => _serviceProvider.GetService(serviceType);

        public void InitAllSingletons()
        {
            var singletonTypes = _serviceCollection.Where(s => s.Lifetime == ServiceLifetime.Singleton).Select(s => s.ServiceType);
            foreach (var type in singletonTypes)
                _serviceProvider.GetRequiredService(type);
        }
    }
}
