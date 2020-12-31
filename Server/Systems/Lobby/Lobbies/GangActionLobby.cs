using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.GamemodesSystem;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.Handler.Maps;
using TDS.Server.LobbySystem.BansHandlers;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Server.LobbySystem.Lobbies.Abstracts;
using TDS.Server.LobbySystem.MapHandlers;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.RoundsHandlers;
using TDS.Server.LobbySystem.Sync;
using TDS.Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies
{
    public class GangActionLobby : RoundFightLobby, IGangActionLobby
    {
        public IBaseGangActionArea GangArea { get; }

        public new IGangActionLobbyTeamsHandler Teams => (IGangActionLobbyTeamsHandler)base.Teams;

        public GangActionLobby(LobbyDb entity, IBaseGangActionArea gangArea, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler, lobbiesHandler, remoteBrowserEventsHandler)
        {
            GangArea = gangArea;
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var settingsHandler = ServiceProvider.GetRequiredService<ISettingsHandler>();
            var mapsLoadingHandler = ServiceProvider.GetRequiredService<MapsLoadingHandler>();
            var gamemodesProvider = ServiceProvider.GetRequiredService<IGamemodesProvider>();

            lobbyDependencies ??= new GangActionLobbyDependencies();

            lobbyDependencies.Bans ??= new GangActionLobbyBansHandler(this, LangHelper);
            lobbyDependencies.Events ??= new RoundFightLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            lobbyDependencies.MapHandler ??=
                new GangActionLobbyMapHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, settingsHandler, mapsLoadingHandler);
            lobbyDependencies.Players ??= new GangActionLobbyPlayers(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            ((GangActionLobbyDependencies)lobbyDependencies).Rounds ??=
                new GangActionLobbyRoundsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, gamemodesProvider);
            lobbyDependencies.Sync ??= new GangActionLobbySync(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new GangActionLobbyTeamsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events,
                LangHelper, settingsHandler, TeamsProvider);

            base.InitDependencies(lobbyDependencies);
        }

        public override async Task Remove()
        {
            await base.Remove().ConfigureAwait(false);
        }
    }
}