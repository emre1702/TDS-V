using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.GamemodesSystem;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.MapVotings;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.Handler.Maps;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Server.LobbySystem.Lobbies.Abstracts;
using TDS.Server.LobbySystem.MapHandlers;
using TDS.Server.LobbySystem.MapVotings;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.RoundsHandlers;
using TDS.Server.LobbySystem.Statistics;
using TDS.Server.LobbySystem.Sync;
using TDS.Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies
{
    public class Arena : RoundFightLobby, IArena
    {
        public new IArenaMapHandler MapHandler => (IArenaMapHandler)base.MapHandler;
        public IArenaMapVoting MapVoting { get; private set; }
        public new IArenaRoundsHandler Rounds => (IArenaRoundsHandler)base.Rounds;
        public new IArenaTeamsHandler Teams => (IArenaTeamsHandler)base.Teams;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Arena(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler, lobbiesHandler, remoteBrowserEventsHandler)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var settingsHandler = ServiceProvider.GetRequiredService<ISettingsHandler>();
            var mapsLoadingHandler = ServiceProvider.GetRequiredService<MapsLoadingHandler>();
            var gamemodesProvider = ServiceProvider.GetRequiredService<IGamemodesProvider>();

            lobbyDependencies ??= new ArenaDependencies();

            lobbyDependencies.Events ??= new RoundFightLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            lobbyDependencies.MapHandler ??= new ArenaMapHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, settingsHandler, mapsLoadingHandler);
            ((ArenaDependencies)lobbyDependencies).MapVoting ??= new ArenaMapVoting(this, mapsLoadingHandler, settingsHandler);
            lobbyDependencies.Players ??= new ArenaPlayers(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            ((ArenaDependencies)lobbyDependencies).Rounds ??= new ArenaRoundsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, gamemodesProvider);
            ((ArenaDependencies)lobbyDependencies).Statistics ??= new ArenaStatistics(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            ((ArenaDependencies)lobbyDependencies).Sync ??= new ArenaSync(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new ArenaTeamsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, LangHelper, TeamsProvider);

            MapVoting = ((ArenaDependencies)lobbyDependencies).MapVoting!;

            base.InitDependencies(lobbyDependencies);
        }
    }
}