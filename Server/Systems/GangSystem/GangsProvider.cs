using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Handler.Events;

namespace TDS.Server.GangsSystem
{
    public class GangsProvider : IGangsProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EventsHandler _eventsHandler;

        public GangsProvider(IServiceProvider serviceProvider, EventsHandler eventsHandler)
        {
            _serviceProvider = serviceProvider;
            _eventsHandler = eventsHandler;

            _ = new GlobalEventsHandler(eventsHandler);
        }

        public IGang GetGang(Gangs entity)
        {
            var gang = _serviceProvider.GetRequiredService<IGang>();
            gang.Init(entity);
            _eventsHandler.OnGangObjectCreated(gang);

            return gang;
        }
    }
}
