using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.GamemodesSystem;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Notifications;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Data.Interfaces.LobbySystem.Rankings;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Spectator;
using TDS_Server.Data.Interfaces.LobbySystem.Statistics;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Maps;
using TDS_Server.LobbySystem.Deathmatch;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Notifications;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.Rankings;
using TDS_Server.LobbySystem.RoundsHandlers;
using TDS_Server.LobbySystem.Spectator;
using TDS_Server.LobbySystem.Statistics;
using TDS_Server.LobbySystem.Sync;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Server.LobbySystem.Weapons;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies.Abstracts
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

        protected RoundFightLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            ILoggingHandler loggingHandler, IServiceProvider serviceProvider)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(entity, databaseHandler, langHelper, eventsHandler, loggingHandler, serviceProvider)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var damageSys = ServiceProvider.GetRequiredService<IDamagesys>();
            var settingsHandler = ServiceProvider.GetRequiredService<ISettingsHandler>();
            var mapsLoadingHandler = ServiceProvider.GetRequiredService<MapsLoadingHandler>();
            var gamemodesProvider = ServiceProvider.GetRequiredService<IGamemodesProvider>();

            lobbyDependencies ??= new RoundFightLobbyDependencies();

            lobbyDependencies.Events ??= new RoundFightLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            lobbyDependencies.Deathmatch ??= new RoundFightLobbyDeathmatch(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, damageSys, LangHelper);
            lobbyDependencies.MapHandler ??= new RoundFightLobbyMapHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, settingsHandler, mapsLoadingHandler);
            lobbyDependencies.Notifications ??= new RoundFightLobbyNotifications(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, LangHelper);
            lobbyDependencies.Players ??= new RoundFightLobbyPlayers(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            ((RoundFightLobbyDependencies)lobbyDependencies).Ranking ??= new RoundFightLobbyRanking(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, settingsHandler);
            ((RoundFightLobbyDependencies)lobbyDependencies).Rounds ??=
                new RoundFightLobbyRoundsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, gamemodesProvider);
            ((RoundFightLobbyDependencies)lobbyDependencies).Spectator ??= new RoundFightLobbySpectator(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            ((RoundFightLobbyDependencies)lobbyDependencies).Statistics ??= new RoundFightLobbyStatistics(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Sync ??= new RoundFightLobbySync(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new RoundFightLobbyTeamsHandler(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events, LangHelper);
            ((RoundFightLobbyDependencies)lobbyDependencies).Weapons ??= new RoundFightLobbyWeapons(this, (IRoundFightLobbyEventsHandler)lobbyDependencies.Events);

            Ranking = ((RoundFightLobbyDependencies)lobbyDependencies).Ranking!;
            Rounds = ((RoundFightLobbyDependencies)lobbyDependencies).Rounds!;
            Statistics = ((RoundFightLobbyDependencies)lobbyDependencies).Statistics!;

            base.InitDependencies(lobbyDependencies);
        }
    }
}
