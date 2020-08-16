using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler.Factories
{
    public class PlayerFactory : IEntityFactory<IPlayer>
    {
        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;

        public PlayerFactory(IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
            => _entitiesByInterfaceCreator = entitiesByInterfaceCreator;

        public IPlayer Create(IntPtr entityPointer, ushort id)
            => _entitiesByInterfaceCreator.Create<ITDSPlayer>(entityPointer, id);
    }
}
