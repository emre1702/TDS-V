using AltV.Net.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler
{
    public class TDSMarkerHandler
    {
        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;

        public TDSMarkerHandler(IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
        {
            _entitiesByInterfaceCreator = entitiesByInterfaceCreator;
        }

        //Todo: Implement marker
        public ITDSMarker? Create(MarkerType type, Position position, Position direction, DegreeRotation rotation, Position scale, Color color, bool bobUpAndDown, int dimension)
        {
            return _entitiesByInterfaceCreator.Create<ITDSMarker>(type, position, direction, rotation, scale, color, bobUpAndDown, dimension);
        }
    }
}
