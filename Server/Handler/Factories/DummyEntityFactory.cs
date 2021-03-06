﻿using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Handler.Entities.GTA.DummyEntities;

namespace TDS.Server.Handler.Factories
{
    public class DummyEntityFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DummyEntityFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.DummyEntities.CreateEntity = CreateDummyEntity;
        }

        private DummyEntity CreateDummyEntity(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSDummyEntity>(_serviceProvider, netHandle);
    }
}
