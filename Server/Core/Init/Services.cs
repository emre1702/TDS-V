using System;
using BonusBotConnector.Client;
using BonusBotConnector_Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Database;
using TDS_Server.Database.Entity;
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
using TDS_Shared.Core;

namespace TDS_Server.Core.Init
{
    internal static class ServicesA
    {
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
            serviceProvider.GetRequiredService<RemoteBrowserEventsHandler>();
            serviceProvider.GetRequiredService<LobbyEventsHandler>();

            // GangSystem
            serviceProvider.GetRequiredService<GangHousesHandler>();
            serviceProvider.GetRequiredService<GangLevelsHandler>();
            serviceProvider.GetRequiredService<GangsHandler>();
            serviceProvider.GetRequiredService<GangwarAreasHandler>();
            serviceProvider.GetRequiredService<GangWindowHandler>();

            // Helper
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
            serviceProvider.GetRequiredService<ITDSPlayerHandler>();
            serviceProvider.GetRequiredService<PlayerCharHandler>();
            serviceProvider.GetRequiredService<PlayerCrouchHandler>();
            serviceProvider.GetRequiredService<PlayerFreecamHandler>();

            serviceProvider.GetRequiredService<ServerInfoHandler>();
            serviceProvider.GetRequiredService<ServerStartHandler>();

            serviceProvider.GetRequiredService<CustomLobbyMenuSyncHandler>();
            serviceProvider.GetRequiredService<DataSyncHandler>();

            serviceProvider.GetRequiredService<IUserpanelHandler>();
            serviceProvider.GetRequiredService<UserpanelCommandsHandler>();

            serviceProvider.GetRequiredService<AdminsHandler>();
            serviceProvider.GetRequiredService<IAnnouncementsHandler>();
            serviceProvider.GetRequiredService<AppConfigHandler>();
            serviceProvider.GetRequiredService<ChatHandler>();
            serviceProvider.GetRequiredService<ClothesHandler>();
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
            serviceProvider.GetRequiredService<ChatInfosHandler>();
            serviceProvider.GetRequiredService<WeaponLevelHandler>();
            serviceProvider.GetRequiredService<DatabaseHandler>();
            serviceProvider.GetRequiredService<WorkaroundsHandler>();
        }
    }
}
