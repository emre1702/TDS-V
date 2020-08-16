using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;

namespace TDS_Server.Handler
{
    public class TDSTextLabelHandler
    {
        private readonly IEntitiesByInterfaceCreator _entitiesByInterfaceCreator;

        public TDSTextLabelHandler(IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
        {
            _entitiesByInterfaceCreator = entitiesByInterfaceCreator;
        }

        public ITDSTextLabel Create(string text, Position position, double range, float fontSize, int font, Color color, bool entitySeethrough, int dimension)
        {
            return _entitiesByInterfaceCreator.Create<ITDSTextLabel>(text, position, range, fontSize, font, color, entitySeethrough, dimension);
        }
    }
}
