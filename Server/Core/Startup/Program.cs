using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TDS_Server.Core.Manager.Commands;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Player;

namespace TDS_Server.Core.Startup
{
    public class Program
    {
        public readonly EventsHandler EventsHandler;
        public readonly RemoteEventsHandler RemoteEventsHandler;
        public Dictionary<ulong, ITDSPlayer>.ValueCollection LoggedInPlayers => _tdsPlayerHandler.LoggedInPlayers;

        private readonly IServiceProvider _serviceProvider;
        private readonly TDSPlayerHandler _tdsPlayerHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;
        
        public Program(IModAPI modAPI)
        {
            _modAPI = modAPI;
            _serviceProvider = Services.InitServiceCollection(modAPI);

            EventsHandler = _serviceProvider.GetRequiredService<EventsHandler>();
            RemoteEventsHandler = _serviceProvider.GetRequiredService<RemoteEventsHandler>();
            _tdsPlayerHandler = _serviceProvider.GetRequiredService<TDSPlayerHandler>();
            _loggingHandler = _serviceProvider.GetRequiredService<ILoggingHandler>();

            var mapsLoadingHandler = _serviceProvider.GetRequiredService<MapsLoadingHandler>();
            mapsLoadingHandler.LoadAllMaps();
        }

        public ITDSPlayer? GetTDSPlayer(IPlayer player)
        {
            return _tdsPlayerHandler.GetTDSPlayerIfExists(player);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                _loggingHandler.LogError("CurrentDomain_UnhandledException: "
                    + ((Exception)e.ExceptionObject).GetBaseException().Message,
                    ((Exception)e.ExceptionObject).StackTrace ?? Environment.StackTrace);
            }
            catch
            {
                // ignored
            }
        }

        private void ReadInput()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input.Length == 0)
                    continue;
                if (input[0] == '/')
                    input = input.Substring(1);

                var consolePlayer = ActivatorUtilities.CreateInstance<ITDSPlayer>(_serviceProvider, null);
                consolePlayer.IsConsole = true;

                _modAPI.Thread.RunInMainThread(() => CommandsManager.UseCommand(consolePlayer, input));

            }
        }
    }
}
