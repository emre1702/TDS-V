using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler.Factories
{
    public class ColShapeFactory : IBaseObjectFactory<IColShape>
    {
        private readonly IServiceProvider _serviceProvider;

        public ColShapeFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IColShape Create(IntPtr baseObjectPointer)
        {
            return ActivatorUtilities.CreateInstance<ITDSColShape>(_serviceProvider, baseObjectPointer);
        }
    }
}
