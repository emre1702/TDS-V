using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Threading.Tasks;
using TDS.Server.Core.Init.Services.Creators;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity;
using TDS.Server.Handler;
using TDS.Server.Handler.Account;
using TDS.Server.Handler.Commands.System;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Factories;
using TDS.Server.Handler.GangSystem;
using TDS.Server.Handler.Maps;
using TDS.Server.Handler.Server;
using ObjectFactory = TDS.Server.Handler.Factories.ObjectFactory;

namespace TDS.Server.Core.Init
{
    public class Program : Script
    {
        private ITDSPlayer? _consolePlayerCache;

        private readonly CustomServiceProvider _serviceProvider;
        private readonly ILoggingHandler _loggingHandler;
        private readonly CommandsHandler _commandsHandler;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Program()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            try
            {
                Handler.Extensions.TaskExtensions.IsMainThread = true;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                _serviceProvider = ServiceProviderCreator.Create();
                InitFactories();

                using (var dbContext = _serviceProvider.GetRequiredService<TDSDbContext>())
                {
                    dbContext.Database.Migrate();
                    var connection = (NpgsqlConnection)dbContext.Database.GetDbConnection();
                    connection.Open();
                    connection.ReloadTypes();
                }

                _loggingHandler = _serviceProvider.GetRequiredService<ILoggingHandler>();
                var mapsLoadingHandler = _serviceProvider.GetRequiredService<MapsLoadingHandler>();
                mapsLoadingHandler.LoadAllMaps();

                var codeChecker = ActivatorUtilities.CreateInstance<CodeMistakesChecker>(_serviceProvider);
                if (codeChecker.CheckHasErrors())
                {
                    NAPI.Resource.StopResource("tds");
                    Environment.Exit(1);
                }

                _serviceProvider.GetRequiredService<TimerHandler>();
                _serviceProvider.GetRequiredService<ServerStartHandler>();
                _serviceProvider.GetRequiredService<MapCreatorRewardsHandler>();   // Uses LobbyCreated event, so needs to be before LobbiesHandler

                var gangsHandler = _serviceProvider.GetRequiredService<GangsHandler>();
                gangsHandler.LoadAll();

                var lobbiesHandler = _serviceProvider.GetRequiredService<LobbiesHandler>();
                lobbiesHandler.LoadLobbies();

                var bansHandler = _serviceProvider.GetRequiredService<BansHandler>();
                var settingsHandler = _serviceProvider.GetRequiredService<ISettingsHandler>();
                bansHandler.RefreshServerBansCache(settingsHandler.ServerSettings.ReloadServerBansEveryMinutes);

                var tdsPlayerHandler = _serviceProvider.GetRequiredService<ITDSPlayerHandler>();
                _commandsHandler = _serviceProvider.GetRequiredService<CommandsHandler>();

                _serviceProvider.InitAllSingletons();

                Task.Run(ReadInput);

                var eventsHandler = _serviceProvider.GetRequiredService<EventsHandler>();
                InitRAGE(eventsHandler);
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

        private static void InitRAGE(EventsHandler eventsHandler)
        {
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetGlobalServerChat(false);

            var date = DateTime.UtcNow;
            NAPI.World.SetTime(date.Hour, date.Minute, date.Second);

            eventsHandler.Minute += (_) =>
            {
                date = DateTime.UtcNow;
                NAPI.World.SetTime(date.Hour, date.Minute, date.Second);
            };
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
                        _consolePlayerCache = _serviceProvider.GetRequiredService<IPlayerProvider>().Create(new NetHandle());
                        _consolePlayerCache.IsConsole = true;
                    }

                    NAPI.Task.RunSafe(() => _commandsHandler.UseCommand(_consolePlayerCache, input));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }

        private void InitFactories()
        {
            _ = new BlipFactory(_serviceProvider);
            _ = new CheckpointFactory(_serviceProvider);
            _ = new ColShapeFactory(_serviceProvider);
            _ = new DummyEntityFactory(_serviceProvider);
            _ = new MarkerFactory(_serviceProvider);
            _ = new ObjectFactory(_serviceProvider);
            _ = new PedFactory(_serviceProvider);
            _ = new PickupFactory(_serviceProvider);
            _ = new PlayerFactory(_serviceProvider.GetRequiredService<IPlayerProvider>());
            _ = new TextLabelFactory(_serviceProvider);
            _ = new VehicleFactory(_serviceProvider);
        }
    }
}
