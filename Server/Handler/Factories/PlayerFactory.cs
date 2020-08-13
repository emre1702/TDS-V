using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler.Factories
{
    public class PlayerFactory : IEntityFactory<IPlayer>
    {
        private readonly IServiceProvider _serviceProvider;

        public PlayerFactory(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public IPlayer Create(IntPtr entityPointer, ushort id)
            => ActivatorUtilities.CreateInstance<ITDSPlayer>(_serviceProvider, entityPointer, id);
    }
}
