using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler.Factories
{
    public class VoiceChannelFactory : IBaseObjectFactory<IVoiceChannel>
    {
        private readonly IServiceProvider _serviceProvider;

        public VoiceChannelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IVoiceChannel Create(IntPtr baseObjectPointer)
        {
            return ActivatorUtilities.CreateInstance<ITDSVoiceChannel>(_serviceProvider, baseObjectPointer);
        }
    }
}
