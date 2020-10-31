using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces.GangsSystem;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;

namespace TDS_Server.GangsSystem
{
    public class GangsProvider : IGangsProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly GangsHandler _gangsHandler;

        public GangsProvider(IServiceProvider serviceProvider, GangsHandler gangsHandler, EventsHandler eventsHandler)
        {
            _serviceProvider = serviceProvider;
            _gangsHandler = gangsHandler;

            new GlobalEventsHandler(eventsHandler);
        }

        public IGang Get(Gangs entity)
        {
            var gang = _serviceProvider.GetRequiredService<IGang>();
            gang.Init(entity);
            _gangsHandler.Add(gang);

            return gang;
        }
    }
}
