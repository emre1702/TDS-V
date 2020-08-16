using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler.Factories
{
    public class VoiceChannelFactory : IBaseObjectFactory<IVoiceChannel>
    {
        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;

        public VoiceChannelFactory(IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
            => _entitiesByInterfaceCreator = entitiesByInterfaceCreator;

        public IVoiceChannel Create(IntPtr baseObjectPointer)
        {
            return _entitiesByInterfaceCreator.Create<ITDSVoiceChannel>(baseObjectPointer);
        }
    }
}
