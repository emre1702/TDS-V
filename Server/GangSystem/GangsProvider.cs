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
        private readonly EventsHandler _eventsHandler;

        public GangsProvider(IServiceProvider serviceProvider, EventsHandler eventsHandler)
        {
            _serviceProvider = serviceProvider;
            _eventsHandler = eventsHandler;

            new GlobalEventsHandler(eventsHandler);
        }

        public IGang Get(Gangs entity)
        {
            var gang = _serviceProvider.GetRequiredService<IGang>();
            gang.Init(entity);
            _eventsHandler.OnGangObjectCreated(gang);

            return gang;
        }
    }
}
