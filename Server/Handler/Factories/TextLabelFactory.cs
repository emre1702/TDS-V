﻿using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Handler.Entities.GTA.TextLabels;

namespace TDS.Server.Handler.Factories
{
    public class TextLabelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public TextLabelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RAGE.Entities.TextLabels.CreateEntity = CreateTextLabel;
        }

        private TextLabel CreateTextLabel(NetHandle netHandle)
            => ActivatorUtilities.CreateInstance<TDSTextLabel>(_serviceProvider, netHandle);
    }
}
