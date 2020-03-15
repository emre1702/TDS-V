using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Player;

namespace TDS_Server.Core.Startup
{
    public class Program
    {
        public readonly EventsHandler EventsHandler;
        public Dictionary<ulong, TDSPlayer>.ValueCollection LoggedInPlayers => _tdsPlayerHandler.LoggedInPlayers;

        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public Program(IModAPI modAPI)
        {
            var serviceProvider = Services.InitServiceCollection(modAPI);

            EventsHandler = serviceProvider.GetRequiredService<EventsHandler>();
            _tdsPlayerHandler = serviceProvider.GetRequiredService<TDSPlayerHandler>();

            foreach (var a in LoggedInPlayers)
            {
                a.
            }
        }

    }
}
