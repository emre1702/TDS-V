using BonusBotConnector.Client;
using BonusBotConnector_Server;
using Microsoft.Extensions.DependencyInjection;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Server.Handler;
using TDS.Server.Handler.Account;
using TDS.Server.Handler.Browser;
using TDS.Server.Handler.Commands.Admin;
using TDS.Server.Handler.Commands.System;
using TDS.Server.Handler.Commands.User;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.FakePickups;
using TDS.Server.Handler.GangSystem;
using TDS.Server.Handler.Helper;
using TDS.Server.Handler.Maps;
using TDS.Server.Handler.PlayerHandlers;
using TDS.Server.Handler.Server;
using TDS.Server.Handler.Sync;
using TDS.Server.Handler.Userpanel;
using TDS.Server.LobbySystem.EventsHandlers;

namespace TDS.Server.Core.Init.Services.Creators
{
    internal static class Handler
    {
        public static IServiceCollection WithHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .WithBonusBotConnectors()
                .WithAccounts()
                .WithBrowser()
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
                .WithFakePickup()
                .WithMisc()
                .WithMailsystem()
                .WithGang()
                .WithDamage();
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

        private static IServiceCollection WithBrowser(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<AngularConstantsProvider>();

        private static IServiceCollection WithCommands(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<CommandsLoader>()
               .AddSingleton<CommandsHandler>()
               .WithAdminCommands()
               .WithUserCommands();
        }

        private static IServiceCollection WithAdminCommands(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<AdminAdminTeamCommands>()
                .AddSingleton<AdminBanCommands>()
                .AddSingleton<AdminChatCommands>()
                .AddSingleton<AdminGangCommands>()
                .AddSingleton<AdminKickCommands>()
                .AddSingleton<AdminLobbyCommands>()
                .AddSingleton<AdminMapCommands>()
                .AddSingleton<AdminMuteCommands>()
                .AddSingleton<AdminTestCommands>();
        }

        private static IServiceCollection WithUserCommands(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<UserChatCommands>()
               .AddSingleton<UserDeathmatchCommands>()
               .AddSingleton<UserInfoCommands>()
               .AddSingleton<UserLobbyCommands>()
               .AddSingleton<UserMapCommands>()
               .AddSingleton<UserMoneyCommands>()
               .AddSingleton<UserPrivateChatCommands>()
               .AddSingleton<UserRelationCommands>();
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
               .AddSingleton<GangActionAreasHandler>()
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
               .AddSingleton<MapsRatingsHandler>()
               .AddSingleton<MapCreatorRewardsHandler>();
        }

        private static IServiceCollection WithPlayers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<ConnectedHandler>()
               .AddSingleton<ITDSPlayerHandler, TDSPlayerHandler>()
               .AddSingleton<PlayerCharHandler>()
               .AddSingleton<PlayerCrouchHandler>()
               .AddSingleton<PlayerFreecamHandler>()
               .AddSingleton<PlayerSettingsSyncHandler>();
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

        private static IServiceCollection WithFakePickup(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<FakePickupsHandler>();
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
               .AddSingleton<IChangelogsHandler, ChangelogsHandler>();
        }
    }
}
