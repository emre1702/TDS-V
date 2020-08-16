using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler.Factories
{
    public class ColShapeFactory : IBaseObjectFactory<IColShape>
    {
        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;

        public ColShapeFactory(IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
        {
            _entitiesByInterfaceCreator = entitiesByInterfaceCreator;
        }

        public IColShape Create(IntPtr baseObjectPointer)
        {
            return _entitiesByInterfaceCreator.Create<ITDSColShape>(baseObjectPointer);
        }
    }
}
