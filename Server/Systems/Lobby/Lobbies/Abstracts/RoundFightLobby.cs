using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.DamageSystem;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.GamemodesSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Notifications;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Data.Interfaces.LobbySystem.Rankings;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Spectator;
using TDS.Server.Data.Interfaces.LobbySystem.Statistics;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Weapons;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.Handler.Maps;
using TDS.Server.LobbySystem.Deathmatch;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Server.LobbySystem.MapHandlers;
using TDS.Server.LobbySystem.Notifications;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.Rankings;
using TDS.Server.LobbySystem.RoundsHandlers;
using TDS.Server.LobbySystem.Spectator;
using TDS.Server.LobbySystem.Statistics;
using TDS.Server.LobbySystem.Sync;
using TDS.Server.LobbySystem.TeamHandlers;
using TDS.Server.LobbySystem.Weapons;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies.Abstracts
{
    public abstract class RoundFightLobby : FightLobby, IRoundFightLobby
    {
        public new IRoundFightLobbyDeathmatch Deathmatch => (IRoundFightLobbyDeathmatch)base.Deathmatch;
        public new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;
        public new IRoundFightLobbyMapHandler MapHandler => (IRoundFightLobbyMapHandler)base.MapHandler;
        public new IRoundFightLobbyNotifications Notifications => (IRoundFightLobbyNotifications)base.Notifications;
        public new IRoundFightLobbyPlayers Players => (IRoundFightLobbyPlayers)base.Players;
        public IRoundFightLobbyRanking Ranking { get; private set; }
        public IRoundFightLobbyRoundsHandler Rounds { get; private set; }
        public new IRoundFightLobbySpectator Spectator => (IRoundFightLobbySpectator)base.Spectator;
        public IRoundFightLobbyStatistics Statistics { get; private set; }
        public new IRoundFightLobbyTeamsHandler Teams => (IRoundFightLobbyTeamsHandler)base.Teams;
        public new IRoundFightLobbyWeapons Weapons => (IRoundFightLobbyWeapons)base.Weapons;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected RoundFightLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider,ILoggingHandler loggingHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var settingsHandler = ServiceProvider.GetRequiredService<ISettingsHandler>();
            var mapsLoadingHandler = ServiceProvider.GetRequiredService<MapsLoadingHandler>();
            var gamemodesProvider = ServiceProvider.GetRequiredService<IGamemodesProvider>();

            lobbyDependencies ??= new RoundFightLobbyDependencies();

            lobbyDependencies.Events ??= new RoundFightLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            ((RoundFightLobbyDependencies)lobbyDependencies).DamageHandler ??= ServiceProvider.GetRequiredService<IDamageHandler>();
            lobbyDependencies.Deathmatch ??= new RoundFightLobbyDeathmatch(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, 
                ((RoundFightLobbyDependencies)lobbyDependencies).DamageHandler!, LangHelper);
            lobbyDependencies.MapHandler ??= new RoundFightLobbyMapHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, settingsHandler, mapsLoadingHandler);
            lobbyDependencies.Notifications ??= new RoundFightLobbyNotifications(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, LangHelper);
            lobbyDependencies.Players ??= new RoundFightLobbyPlayers(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            ((RoundFightLobbyDependencies)lobbyDependencies).Ranking ??= new RoundFightLobbyRanking(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, settingsHandler);
            ((RoundFightLobbyDependencies)lobbyDependencies).Rounds ??=
                new RoundFightLobbyRoundsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, gamemodesProvider);
            ((RoundFightLobbyDependencies)lobbyDependencies).Spectator ??= new RoundFightLobbySpectator(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            ((RoundFightLobbyDependencies)lobbyDependencies).Statistics ??= new RoundFightLobbyStatistics(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Sync ??= new RoundFightLobbySync(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new RoundFightLobbyTeamsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, LangHelper, TeamsProvider);
            ((RoundFightLobbyDependencies)lobbyDependencies).Weapons ??= new RoundFightLobbyWeapons(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);

            Ranking = ((RoundFightLobbyDependencies)lobbyDependencies).Ranking!;
            Rounds = ((RoundFightLobbyDependencies)lobbyDependencies).Rounds!;
            Statistics = ((RoundFightLobbyDependencies)lobbyDependencies).Statistics!;

            base.InitDependencies(lobbyDependencies);
        }
    }
}
