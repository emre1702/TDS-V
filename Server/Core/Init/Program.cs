using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Commands;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Player;

namespace TDS_Server.Core.Init
{
    public class Program
    {
        public readonly EventsHandler EventsHandler;
        public readonly RemoteEventsHandler RemoteEventsHandler;
        public Dictionary<ulong, ITDSPlayer>.ValueCollection LoggedInPlayers => _tdsPlayerHandler.LoggedInPlayers;

        private TDSPlayer? _consolePlayerCache;

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

            var codeChecker = ActivatorUtilities.CreateInstance<CodeMistakesChecker>(_serviceProvider);
            if (codeChecker.CheckHasErrors())
            {
                modAPI.Resource.StopThis();
                Environment.Exit(1);
            }

            var lobbiesHandler = _serviceProvider.GetRequiredService<LobbiesHandler>();
            lobbiesHandler.LoadLobbies();

            var bansHandler = _serviceProvider.GetRequiredService<BansHandler>();
            var settingsHandler = _serviceProvider.GetRequiredService<ISettingsHandler>();
            bansHandler.RefreshServerBansCache((ulong)settingsHandler.ServerSettings.ReloadServerBansEveryMinutes);

            var gangsHandler = _serviceProvider.GetRequiredService<GangsHandler>();
            gangsHandler.LoadAll();

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
                try
                {
                    string input = Console.ReadLine();
                    if (input is null)
                        return;
                    if (input.Length == 0)
                        continue;
                    if (input[0] == '/')
                        input = input.Substring(1);

                    if (_consolePlayerCache is null)
                    {
                        _consolePlayerCache = ActivatorUtilities.CreateInstance<TDSPlayer>(_serviceProvider);
                        _consolePlayerCache.IsConsole = true;
                    }

                    _modAPI.Thread.RunInMainThread(() => _commandsHandler.UseCommand(_consolePlayerCache, input));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);
                }

            }
        }
    }
}
