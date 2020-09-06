using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Commands;
using TDS_Server.Handler.Entities.GTA.GTAPlayer;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Factories;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Server;
using ObjectFactory = TDS_Server.Handler.Factories.ObjectFactory;

namespace TDS_Server.Core.Init
{
    public class Program : Script
    {
        public readonly EventsHandler EventsHandler;
        public readonly RemoteBrowserEventsHandler RemoteBrowserEventsHandler;
        public readonly LobbiesHandler LobbiesHandler;
        public ICollection<ITDSPlayer> LoggedInPlayers => _tdsPlayerHandler.LoggedInPlayers;

        private ITDSPlayer? _consolePlayerCache;

        private readonly IServiceProvider _serviceProvider;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly CommandsHandler _commandsHandler;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Program()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                _serviceProvider = Services.InitServiceCollection();
                InitFactories();

                using (var dbContext = _serviceProvider.GetRequiredService<TDSDbContext>())
                {
                    dbContext.Database.Migrate();
                    var connection = (NpgsqlConnection)dbContext.Database.GetDbConnection();
                    connection.Open();
                    connection.ReloadTypes();
                }

                var mapsLoadingHandler = _serviceProvider.GetRequiredService<MapsLoadingHandler>();
                mapsLoadingHandler.LoadAllMaps();

                var codeChecker = ActivatorUtilities.CreateInstance<CodeMistakesChecker>(_serviceProvider);
                if (codeChecker.CheckHasErrors())
                {
                    NAPI.Resource.StopResource("tds");
                    Environment.Exit(1);
                }

                EventsHandler = _serviceProvider.GetRequiredService<EventsHandler>();
                _serviceProvider.GetRequiredService<ServerStartHandler>();

                LobbiesHandler = _serviceProvider.GetRequiredService<LobbiesHandler>();
                LobbiesHandler.LoadLobbies();

                var bansHandler = _serviceProvider.GetRequiredService<BansHandler>();
                var settingsHandler = _serviceProvider.GetRequiredService<ISettingsHandler>();
                bansHandler.RefreshServerBansCache(settingsHandler.ServerSettings.ReloadServerBansEveryMinutes);

                var gangsHandler = _serviceProvider.GetRequiredService<GangsHandler>();
                gangsHandler.LoadAll();

                RemoteBrowserEventsHandler = _serviceProvider.GetRequiredService<RemoteBrowserEventsHandler>();
                _tdsPlayerHandler = _serviceProvider.GetRequiredService<ITDSPlayerHandler>();
                _loggingHandler = _serviceProvider.GetRequiredService<ILoggingHandler>();
                _commandsHandler = _serviceProvider.GetRequiredService<CommandsHandler>();

                Services.InitializeSingletons(_serviceProvider);

                var loggingHandler = _serviceProvider.GetRequiredService<ILoggingHandler>();
                loggingHandler.SetTDSPlayerHandler(_tdsPlayerHandler);

                Task.Run(ReadInput);
            }
            catch (Exception ex)
            {
                if (_loggingHandler is { })
                    _loggingHandler.LogError(ex);
                else
                    Console.WriteLine(ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);
#if RELEASE
                Environment.Exit(1);
#else
                Console.ReadKey();
#endif
            }
        }

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
                    string? input = Console.ReadLine();
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

                    NAPI.Task.Run(() => _commandsHandler.UseCommand(_consolePlayerCache, input));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }

        private void InitFactories()
        {
            new BlipFactory(_serviceProvider);
            new CheckpointFactory(_serviceProvider);
            new ColShapeFactory(_serviceProvider);
            new DummyEntityFactory(_serviceProvider);
            new MarkerFactory(_serviceProvider);
            new ObjectFactory(_serviceProvider);
            new PedFactory(_serviceProvider);
            new PickupFactory(_serviceProvider);
            new PlayerFactory(_serviceProvider);
            new TextLabelFactory(_serviceProvider);
            new VehicleFactory(_serviceProvider);
        }
    }
}
