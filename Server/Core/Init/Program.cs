using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Factories;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Server;
using TDS_Shared.Core;

namespace TDS_Server.Core.Init
{
    public class Program : AsyncResource
    {
        private readonly ILoggingHandler? _loggingHandler;
        private readonly EventsHandler? _eventsHandler;
        private readonly IServiceProvider _serviceProvider;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public Program()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                var serviceProvider = Services.InitServiceCollection();

                using (var dbContext = serviceProvider.GetRequiredService<TDSDbContext>())
                {
                    dbContext.Database.Migrate();
                    var connection = (NpgsqlConnection)dbContext.Database.GetDbConnection();
                    connection.Open();
                    connection.ReloadTypes();
                }

                var mapsLoadingHandler = serviceProvider.GetRequiredService<MapsLoadingHandler>();
                mapsLoadingHandler.LoadAllMaps();

                var codeChecker = ActivatorUtilities.CreateInstance<CodeMistakesChecker>(serviceProvider);
                if (codeChecker.CheckHasErrors())
                {
                    Environment.Exit(1);
                }

                _eventsHandler = serviceProvider.GetRequiredService<EventsHandler>();
                serviceProvider.GetRequiredService<ServerStartHandler>();

                var lobbiesHandler = serviceProvider.GetRequiredService<LobbiesHandler>();
                lobbiesHandler.LoadLobbies();

                var bansHandler = serviceProvider.GetRequiredService<BansHandler>();
                var settingsHandler = serviceProvider.GetRequiredService<ISettingsHandler>();
                bansHandler.RefreshServerBansCache(settingsHandler.ServerSettings.ReloadServerBansEveryMinutes);

                var gangsHandler = serviceProvider.GetRequiredService<GangsHandler>();
                gangsHandler.LoadAll();

                var remoteBrowserEventsHandler = serviceProvider.GetRequiredService<RemoteBrowserEventsHandler>();
                _loggingHandler = serviceProvider.GetRequiredService<ILoggingHandler>();

                Services.InitializeSingletons(serviceProvider);
            }
            catch (Exception ex)
            {
                if (_loggingHandler is { })
                    _loggingHandler.LogError(ex);
                else
                    Console.WriteLine(ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);
                Environment.Exit(1);
            }
            finally
            {
                #if DEBUG 
                Console.ReadLine();
                #endif
            }
        }

        public override void OnStart()
        {
            
        }

        public override void OnStop()
        {
            _eventsHandler?.OnResourceStop();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
            => new VehicleFactory(_serviceProvider);

        public override IEntityFactory<IPlayer> GetPlayerFactory()
            => new PlayerFactory(_serviceProvider);

        public override IBaseObjectFactory<IColShape> GetColShapeFactory()
            => new ColShapeFactory(_serviceProvider);

        public override IBaseObjectFactory<IVoiceChannel> GetVoiceChannelFactory()
            => new VoiceChannelFactory(_serviceProvider);

        public override void OnTick()
            => TDSTimer.OnUpdateFunc();

        public void HandleProgramException(Exception ex, string msgBefore = "")
        {
            _loggingHandler?.LogError($"{msgBefore}{Environment.NewLine}{ex.GetBaseException().Message}");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleProgramException((Exception)e.ExceptionObject, "CurrentDomain_UnhandledException: ");
        }

        //Todo: Add ScriptEventType.ConsoleCommand   (string command, string[] args)
        /*private async void ReadInput()
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
                        _consolePlayerCache = ActivatorUtilities.CreateInstance<TDSPlayer>(serviceProvider);
                        _consolePlayerCache.IsConsole = true;
                    }

                    await AltAsync.Do(() => _commandsHandler.UseCommand(_consolePlayerCache, input));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }*/
    }
}
