using AltV.Net.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler
{
    public class TDSBlipHandler
    {
        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;

        public TDSBlipHandler(IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
        {
            _entitiesByInterfaceCreator = entitiesByInterfaceCreator;
        }

        public ITDSBlip Create(uint sprite, Position position, float scale = 1f, byte color = 0, string name = "", byte alpha = 255, 
            float drawDistance = 0, bool shortRange = false, int dimension = 0)
            => _entitiesByInterfaceCreator.Create<ITDSBlip>(sprite, position, scale, color, name, alpha, drawDistance, shortRange, dimension);
    }
}
