using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.Utility
{
    public class CustomServiceProvider : IServiceProvider
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly ServiceProvider _serviceProvider;

        public CustomServiceProvider(IServiceCollection collection)
        {
            _serviceCollection = collection;
            collection.AddSingleton(this);
            _serviceProvider = collection.BuildServiceProvider();
        }

        public object GetService(Type serviceType)
            => _serviceProvider.GetService(serviceType);

        public void InitAllSingletons()
        {
            var logger = _serviceProvider.GetRequiredService<ILoggingHandler>();
            try
            {
                var singletonTypes = GetAllSingletonTypes();
                foreach (var type in singletonTypes)
                    _serviceProvider.GetRequiredService(type);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }
        }

        public IEnumerable<Type> GetAllSingletonTypes()
            => _serviceCollection.Where(s => s.Lifetime == ServiceLifetime.Singleton).Select(s => s.ServiceType);
    }
}
