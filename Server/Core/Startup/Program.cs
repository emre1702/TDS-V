using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Events.Mod;

namespace TDS_Server.Core.Startup
{
    public class Program
    {
        public EventsHandler EventsHandler;

        public Program(IModAPI modAPI)
        {
            var serviceProvider = Services.InitServiceCollection(modAPI);

            EventsHandler = serviceProvider.GetRequiredService<EventsHandler>();
        }

    }
}
