using BonusBotConnector.Client;
using BonusBotConnector_Server;
using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Core.Damagesystem;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Commands;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.PlayerHandlers;
using TDS_Server.Handler.Server;
using TDS_Server.Handler.Sync;
using TDS_Server.Handler.Userpanel;
using TDS_Server.LobbySystem.EventsHandlers;

namespace TDS_Server.Core.Init.Services.Creators
{
    internal static class Handler
    {
        public static IServiceCollection WithHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .WithBonusBotConnectors()
                .WithAccounts()
                .WithCommands()
                .WithEvents()
                .WithGangSystems()
                .WithPlayerSystem()
                .WithHelper()
                .WithMaps()
                .WithPlayers()
                .WithServer()
                .WithSync()
                .WithUserpanel()
                .WithMisc()
                .WithMailsystem();
        }

        private static IServiceCollection WithBonusBotConnectors(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<BonusBotConnectorClient>()
               .AddSingleton<BonusBotConnectorServer>();
        }

        private static IServiceCollection WithAccounts(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<BansHandler>()
               .AddSingleton<LoginHandler>()
               .AddSingleton<RegisterHandler>()
               .AddSingleton<ResetPasswordHandler>();
        }

        private static IServiceCollection WithCommands(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<CommandsHandler>()
               .AddSingleton<BaseCommands>();
        }

        private static IServiceCollection WithEvents(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<EventsHandler>()
               .AddSingleton<AllLobbiesEventsHandler>()
               .AddSingleton<AllLobbiesRemoteEventsHandler>()
               .AddSingleton<RemoteBrowserEventsHandler>();
        }

        private static IServiceCollection WithGangSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<GangHousesHandler>()
               .AddSingleton<GangLevelsHandler>()
               .AddSingleton<GangsHandler>()
               .AddSingleton<GangwarAreasHandler>()
               .AddSingleton<GangWindowHandler>();
        }

        private static IServiceCollection WithHelper(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<ChallengesHelper>()
               .AddSingleton<DatabasePlayerHelper>()
               .AddSingleton<LangHelper>()
               .AddSingleton<NameCheckHelper>()
               .AddSingleton<XmlHelper>();
        }

        private static IServiceCollection WithMaps(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<MapCreatorHandler>()
               .AddSingleton<MapFavouritesHandler>()
               .AddSingleton<MapsLoadingHandler>()
               .AddSingleton<MapsRatingsHandler>();
        }

        private static IServiceCollection WithPlayers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<ConnectedHandler>()
               .AddSingleton<ITDSPlayerHandler, TDSPlayerHandler>()
               .AddSingleton<PlayerCharHandler>()
               .AddSingleton<PlayerCrouchHandler>()
               .AddSingleton<PlayerFreecamHandler>();
        }

        private static IServiceCollection WithServer(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<ServerInfoHandler>()
               .AddSingleton<ServerStartHandler>();
        }

        private static IServiceCollection WithSync(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<CustomLobbyMenuSyncHandler>()
                .AddSingleton<DataSyncHandler>();
        }

        private static IServiceCollection WithUserpanel(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IUserpanelHandler, UserpanelHandler>()
               .AddSingleton<UserpanelCommandsHandler>();
        }

        private static IServiceCollection WithMisc(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<AdminsHandler>()
               .AddSingleton<IAnnouncementsHandler, AnnouncementsHandler>()
               .AddSingleton<AppConfigHandler>()
               .AddSingleton<ChatHandler>()
               .AddSingleton<ClothesHandler>()
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
               .AddSingleton<ChatInfosHandler>()
               .AddSingleton<WeaponLevelHandler>()
               .AddTransient<IDatabaseHandler, DatabaseHandler>()
               .AddSingleton<WorkaroundsHandler>()
               .AddSingleton<FreeroamDataHandler>()
               .AddTransient<IDamagesys, Damagesys>();
        }
    }
}
