using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Database.Entity;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Commands;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Player;
using TDS_Server.Handler.Server;

namespace TDS_Server.Core.Init
{
    public class Program
    {
        public readonly EventsHandler EventsHandler;
        public readonly RemoteEventsHandler RemoteEventsHandler;
        public readonly RemoteBrowserEventsHandler RemoteBrowserEventsHandler;
        public readonly LobbiesHandler LobbiesHandler;
        public ICollection<ITDSPlayer> LoggedInPlayers => _tdsPlayerHandler.LoggedInPlayers;

        private TDSPlayer? _consolePlayerCache;

        private readonly IServiceProvider _serviceProvider;
        private readonly TDSPlayerHandler _tdsPlayerHandler;
        private readonly TDSVehicleHandler _tdsVehicleHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;
        private readonly CommandsHandler _commandsHandler;



#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public Program(IModAPI modAPI)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                _modAPI = modAPI;
                _serviceProvider = Services.InitServiceCollection(modAPI);

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
                    modAPI.Resource.StopThis();
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

                
                RemoteEventsHandler = _serviceProvider.GetRequiredService<RemoteEventsHandler>();
                RemoteBrowserEventsHandler = _serviceProvider.GetRequiredService<RemoteBrowserEventsHandler>();
                _tdsPlayerHandler = _serviceProvider.GetRequiredService<TDSPlayerHandler>();
                _tdsVehicleHandler = _serviceProvider.GetRequiredService<TDSVehicleHandler>();
                _loggingHandler = _serviceProvider.GetRequiredService<ILoggingHandler>();
                _commandsHandler = _serviceProvider.GetRequiredService<CommandsHandler>();

                Services.InitializeSingletons(_serviceProvider);

                Task.Run(ReadInput);
            }
            catch (Exception ex)
            {
                if (_loggingHandler is { })
                    _loggingHandler.LogError(ex);
                else
                    Console.WriteLine(ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);
                Environment.Exit(1);
            }

        }

        public ITDSPlayer? GetTDSPlayerIfLoggedIn(IPlayer player)
            => _tdsPlayerHandler.GetIfLoggedIn(player);

        public ITDSPlayer? GetTDSPlayerIfLoggedIn(ushort remoteId)
            => _tdsPlayerHandler.GetIfLoggedIn(remoteId);

        public ITDSPlayer GetTDSPlayer(IPlayer player)
            => _tdsPlayerHandler.Get(player);

        public ITDSVehicle GetTDSVehicle(IVehicle vehicle) 
            => _tdsVehicleHandler.Get(vehicle); 

        public ITDSPlayer GetNotLoggedInTDSPlayer(IPlayer player)
            => _tdsPlayerHandler.GetNotLoggedIn(player);

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
