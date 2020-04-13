using BonusBotConnector.Client;
using BonusBotConnector_Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Core.Manager.Timer;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database;
using TDS_Server.Database.Entity;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Commands;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Player;
using TDS_Server.Handler.Server;
using TDS_Server.Handler.Sync;
using TDS_Server.Handler.Userpanel;
using TDS_Shared.Core;

namespace TDS_Server.Core.Init
{
    internal static class Services
    {

        internal static ServiceProvider InitServiceCollection(IModAPI modAPI)
        {
            var appConfigHandler = new AppConfigHandler();

            var loggerFactory = LoggerFactory.Create(builder =>
                    builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug)
                        .AddProvider(new CustomDBLogger(@"D:\DBLogs\FromCsharp\log.txt"))
                );

            var serviceCollection = new ServiceCollection();

            serviceCollection
               .AddSingleton(modAPI)

               .AddSingleton<BonusBotConnectorClient>()
               .AddSingleton<BonusBotConnectorServer>()

               // Account
               .AddSingleton<BansHandler>()
               .AddSingleton<LoginHandler>()
               .AddSingleton<RegisterHandler>()

               // Commands
               .AddSingleton<CommandsHandler>()
               .AddSingleton<BaseCommands>()

               // Events
               .AddSingleton<EventsHandler>()
               .AddSingleton<RemoteEventsHandler>()
               .AddSingleton<RemoteBrowserEventsHandler>()

               // Helper
               .AddSingleton<ChallengesHelper>()
               .AddSingleton<DatabasePlayerHelper>()
               .AddSingleton<LangHelper>()
               .AddSingleton<NameCheckHelper>()
               .AddSingleton<XmlHelper>()

               // Maps 
               .AddSingleton<MapCreatorHandler>()
               .AddSingleton<MapFavouritesHandler>()
               .AddSingleton<MapsLoadingHandler>()
               .AddSingleton<MapsRatingsHandler>()

               // Player
               .AddSingleton<ConnectedHandler>()
               .AddSingleton<TDSPlayerHandler>()

               // Server
               .AddSingleton<ServerInfoHandler>()
               .AddSingleton<ServerStartHandler>()

               // Sync
               .AddSingleton<CustomLobbyMenuSyncHandler>()
               .AddSingleton<DataSyncHandler>()

               // Userpanel
               .AddSingleton<UserpanelHandler>()
               .AddSingleton<UserpanelCommandsHandler>()

               .AddSingleton<AdminsHandler>()
               .AddSingleton<IAnnouncementsHandler, AnnouncementsHandler>()
               .AddSingleton<AppConfigHandler>()
               .AddSingleton<ChatHandler>()
               .AddSingleton<ClothesHandler>()
               .AddSingleton<GangsHandler>()
               .AddSingleton<GangwarAreasHandler>()
               .AddSingleton<InvitationsHandler>()
               .AddSingleton<LobbiesHandler>()
               .AddSingleton<ILoggingHandler, LoggingHandler>()
               .AddSingleton<MappingHandler>()
               .AddSingleton<OfflineMessagesHandler>()
               .AddSingleton<ResourceStopHandler>()
               .AddSingleton<ServerStatsHandler>()
               .AddSingleton<ISettingsHandler, SettingsHandler>()
               .AddSingleton<SpectateHandler>()
               .AddSingleton<TimerHandler>()
               .AddSingleton<WeaponDatasLoadingHandler>()
               .AddSingleton<ScoreboardHandler>()


               .AddSingleton<Serializer>()


               .AddDbContext<TDSDbContext>(options => InitDbContextOptionsBuilder(options, appConfigHandler, loggerFactory), ServiceLifetime.Transient, ServiceLifetime.Singleton);

            return serviceCollection.BuildServiceProvider();
        }

        internal static void InitializeSingletons(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<BonusBotConnectorClient>();
            serviceProvider.GetRequiredService<BonusBotConnectorServer>();

            serviceProvider.GetRequiredService<BansHandler>();
            serviceProvider.GetRequiredService<LoginHandler>();
            serviceProvider.GetRequiredService<RegisterHandler>();

            serviceProvider.GetRequiredService<CommandsHandler>();
            serviceProvider.GetRequiredService<BaseCommands>();

            serviceProvider.GetRequiredService<EventsHandler>();
            serviceProvider.GetRequiredService<RemoteEventsHandler>();
            serviceProvider.GetRequiredService<RemoteBrowserEventsHandler>();

            serviceProvider.GetRequiredService<ChallengesHelper>();
            serviceProvider.GetRequiredService<DatabasePlayerHelper>();
            serviceProvider.GetRequiredService<LangHelper>();
            serviceProvider.GetRequiredService<NameCheckHelper>();
            serviceProvider.GetRequiredService<XmlHelper>();

            serviceProvider.GetRequiredService<MapCreatorHandler>();
            serviceProvider.GetRequiredService<MapFavouritesHandler>();
            serviceProvider.GetRequiredService<MapsLoadingHandler>();
            serviceProvider.GetRequiredService<MapsRatingsHandler>();

            serviceProvider.GetRequiredService<ConnectedHandler>();
            serviceProvider.GetRequiredService<TDSPlayerHandler>();

            serviceProvider.GetRequiredService<ServerInfoHandler>();
            serviceProvider.GetRequiredService<ServerStartHandler>();

            serviceProvider.GetRequiredService<CustomLobbyMenuSyncHandler>();
            serviceProvider.GetRequiredService<DataSyncHandler>();

            serviceProvider.GetRequiredService<UserpanelHandler>();

            serviceProvider.GetRequiredService<AdminsHandler>();
            serviceProvider.GetRequiredService<IAnnouncementsHandler>();
            serviceProvider.GetRequiredService<AppConfigHandler>();
            serviceProvider.GetRequiredService<ChatHandler>();
            serviceProvider.GetRequiredService<ClothesHandler>();
            serviceProvider.GetRequiredService<GangsHandler>();
            serviceProvider.GetRequiredService<GangwarAreasHandler>();
            serviceProvider.GetRequiredService<InvitationsHandler>();
            serviceProvider.GetRequiredService<LobbiesHandler>();
            serviceProvider.GetRequiredService<ILoggingHandler>();
            serviceProvider.GetRequiredService<MappingHandler>();
            serviceProvider.GetRequiredService<OfflineMessagesHandler>();
            serviceProvider.GetRequiredService<ResourceStopHandler>();
            serviceProvider.GetRequiredService<ServerStatsHandler>();
            serviceProvider.GetRequiredService<ISettingsHandler>();
            serviceProvider.GetRequiredService<SpectateHandler>();
            serviceProvider.GetRequiredService<TimerHandler>();
            serviceProvider.GetRequiredService<WeaponDatasLoadingHandler>();
            serviceProvider.GetRequiredService<ScoreboardHandler>();
            serviceProvider.GetRequiredService<UserpanelCommandsHandler>();
        }

        internal static void InitDbContextOptionsBuilder(DbContextOptionsBuilder options, AppConfigHandler appConfigHandler, ILoggerFactory? loggerFactory)
        {
            options.UseSnakeCaseNamingConvention();
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            if (loggerFactory is { })
                options.UseLoggerFactory(loggerFactory);

            options.UseNpgsql(appConfigHandler.ConnectionString, options =>
                    options.EnableRetryOnFailure());
            // .EnableSensitiveDataLogging()
        }

    }
}
