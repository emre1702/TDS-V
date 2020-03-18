using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using TDS_Server.Core.Manager.Maps;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Player;

namespace TDS_Server.Core.Startup
{
    public class Program
    {
        public readonly EventsHandler EventsHandler;
        public readonly RemoteEventsHandler RemoteEventsHandler;
        public Dictionary<ulong, ITDSPlayer>.ValueCollection LoggedInPlayers => _tdsPlayerHandler.LoggedInPlayers;

        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public Program(IModAPI modAPI)
        {
            var serviceProvider = Services.InitServiceCollection(modAPI);

            EventsHandler = serviceProvider.GetRequiredService<EventsHandler>();
            RemoteEventsHandler = serviceProvider.GetRequiredService<RemoteEventsHandler>();
            _tdsPlayerHandler = serviceProvider.GetRequiredService<TDSPlayerHandler>();

            var mapsLoadingHandler = serviceProvider.GetRequiredService<MapsLoadingHandler>();
            mapsLoadingHandler.LoadAllMaps();
        }

        public ITDSPlayer? GetTDSPlayer(IPlayer player)
        {
            return _tdsPlayerHandler.GetTDSPlayerIfExists(player);
        }
    }
}
