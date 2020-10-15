using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.GamemodesSystem;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.MapVotings;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Maps;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.MapVotings;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.RoundsHandlers;
using TDS_Server.LobbySystem.Statistics;
using TDS_Server.LobbySystem.Sync;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
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
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider)
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var settingsHandler = ServiceProvider.GetRequiredService<ISettingsHandler>();
            var mapsLoadingHandler = ServiceProvider.GetRequiredService<MapsLoadingHandler>();
            var gamemodesProvider = ServiceProvider.GetRequiredService<IGamemodesProvider>();

            lobbyDependencies ??= new ArenaDependencies();

            lobbyDependencies.Events ??= new RoundFightLobbyEventsHandler(this, GlobalEventsHandler);
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
