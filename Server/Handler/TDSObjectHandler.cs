using AltV.Net;
using AltV.Net.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler
{
    public class TDSObjectHandler
    {
        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;

        public TDSObjectHandler(IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
        {
            _entitiesByInterfaceCreator = entitiesByInterfaceCreator;
        }

        public ITDSObject Create(int model, Position position, DegreeRotation rotation, byte alpha, int dimension)
        {
            return _entitiesByInterfaceCreator.Create<ITDSObject>(model, position, rotation, alpha, dimension);
        }

        public ITDSObject Create(string hash, Position position, DegreeRotation rotation, byte alpha, int dimension)
            => Create((int)Alt.Hash(hash), position, rotation, alpha, dimension);
    }
}
