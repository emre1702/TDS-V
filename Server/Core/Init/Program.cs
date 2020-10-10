using System;
using System.Threading.Tasks;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using TDS_Server.Core.Init.Services;
using TDS_Server.Core.Init.Services.Creators;
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
using TDS_Shared.Core;
using ObjectFactory = TDS_Server.Handler.Factories.ObjectFactory;

namespace TDS_Server.Core.Init
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
                Data.Extensions.TaskExtensions.IsMainThread = true;
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

                var mapsLoadingHandler = _serviceProvider.GetRequiredService<MapsLoadingHandler>();
                mapsLoadingHandler.LoadAllMaps();

                var codeChecker = ActivatorUtilities.CreateInstance<CodeMistakesChecker>(_serviceProvider);
                if (codeChecker.CheckHasErrors())
                {
                    NAPI.Resource.StopResource("tds");
                    Environment.Exit(1);
                }

                _serviceProvider.GetRequiredService<ServerStartHandler>();

                var lobbiesHandler = _serviceProvider.GetRequiredService<LobbiesHandler>();
                lobbiesHandler.LoadLobbies();

                var bansHandler = _serviceProvider.GetRequiredService<BansHandler>();
                var settingsHandler = _serviceProvider.GetRequiredService<ISettingsHandler>();
                bansHandler.RefreshServerBansCache(settingsHandler.ServerSettings.ReloadServerBansEveryMinutes);

                var gangsHandler = _serviceProvider.GetRequiredService<GangsHandler>();
                gangsHandler.LoadAll();

                var tdsPlayerHandler = _serviceProvider.GetRequiredService<ITDSPlayerHandler>();
                _loggingHandler = _serviceProvider.GetRequiredService<ILoggingHandler>();
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

        private void InitRAGE(EventsHandler eventsHandler)
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
