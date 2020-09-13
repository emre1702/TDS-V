﻿using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Handler.Entities.GTA.Peds;

namespace TDS_Server.Handler.Factories
{
    public class PedFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PedFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.Peds.CreateEntity = CreatePed;
        }

        private Ped CreatePed(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSPed>(_serviceProvider, netHandle);
    }
}