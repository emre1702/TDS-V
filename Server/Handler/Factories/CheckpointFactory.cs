using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Handler.Entities.GTA.Checkpoints;

namespace TDS_Server.Handler.Factories
{
    public class CheckpointFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CheckpointFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.Checkpoints.CreateEntity = CreateCheckpoint;
        }

        private Checkpoint CreateCheckpoint(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSCheckpoint>(_serviceProvider, netHandle);
    }
}
