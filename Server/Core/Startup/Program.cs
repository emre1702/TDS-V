using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Commands;
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
        private readonly CommandsHandler _commandsHandler;


        public Program(IModAPI modAPI)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;   

            _modAPI = modAPI;
            _serviceProvider = Services.InitServiceCollection(modAPI);

            Services.InitializeSingletons(_serviceProvider);

            EventsHandler = _serviceProvider.GetRequiredService<EventsHandler>();
            RemoteEventsHandler = _serviceProvider.GetRequiredService<RemoteEventsHandler>();
            _tdsPlayerHandler = _serviceProvider.GetRequiredService<TDSPlayerHandler>();
            _loggingHandler = _serviceProvider.GetRequiredService<ILoggingHandler>();
            _commandsHandler = _serviceProvider.GetRequiredService<CommandsHandler>();

            var mapsLoadingHandler = _serviceProvider.GetRequiredService<MapsLoadingHandler>();
            mapsLoadingHandler.LoadAllMaps();

            Task.Run(ReadInput);
        }

        public ITDSPlayer? GetTDSPlayerIfLoggedIn(IPlayer player)
            => _tdsPlayerHandler.GetIfLoggedIn(player);

        public ITDSPlayer? GetTDSPlayer(IPlayer player)
            => _tdsPlayerHandler.Get(player);

        public void HandleProgramException(Exception ex, string msgBefore = "")
        {
            _loggingHandler.LogError($"{msgBefore}{Environment.NewLine}{ex.GetBaseException().Message}");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleProgramException((Exception)e.ExceptionObject, "CurrentDomain_UnhandledException: ");
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

                _modAPI.Thread.RunInMainThread(() => _commandsHandler.UseCommand(consolePlayer, input));

            }
        }
    }
}
