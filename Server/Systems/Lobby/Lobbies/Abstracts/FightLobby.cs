using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.DamageSystem;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Data.Interfaces.LobbySystem.Spectator;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Weapons;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.LobbySystem.Deathmatch;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.Spectator;
using TDS.Server.LobbySystem.TeamHandlers;
using TDS.Server.LobbySystem.Weapons;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies.Abstracts
{
    public abstract class FightLobby : BaseLobby, IFightLobby
    {
        public new IFightLobbyDeathmatch Deathmatch => (IFightLobbyDeathmatch)base.Deathmatch;
        public new IFightLobbyEventsHandler Events => (IFightLobbyEventsHandler)base.Events;
        public new IFightLobbyPlayers Players => (IFightLobbyPlayers)base.Players;
        public IFightLobbySpectator Spectator { get; private set; }
        public new IFightLobbyTeamsHandler Teams => (IFightLobbyTeamsHandler)base.Teams;
        public IFightLobbyWeapons Weapons { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected FightLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler, lobbiesHandler, remoteBrowserEventsHandler)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            lobbyDependencies ??= new FightLobbyDependencies();

            lobbyDependencies.Events ??= new FightLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            ((FightLobbyDependencies)lobbyDependencies).Spectator ??= new FightLobbySpectator(this);
            ((FightLobbyDependencies)lobbyDependencies).Weapons ??= new FightLobbyWeapons(this, (IFightLobbyEventsHandler)lobbyDependencies.Events);
            ((FightLobbyDependencies)lobbyDependencies).DamageHandler ??= ServiceProvider.GetRequiredService<IDamageHandler>();
            lobbyDependencies.Deathmatch ??= new FightLobbyDeathmatch(this, (IFightLobbyEventsHandler)lobbyDependencies.Events,
                ((FightLobbyDependencies)lobbyDependencies).DamageHandler!);
            lobbyDependencies.Players ??= new FightLobbyPlayers(this, (IFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new FightLobbyTeamsHandler(this, (IFightLobbyEventsHandler)lobbyDependencies.Events, LangHelper, TeamsProvider);

            Spectator = ((FightLobbyDependencies)lobbyDependencies).Spectator!;
            Weapons = ((FightLobbyDependencies)lobbyDependencies).Weapons!;

            base.InitDependencies(lobbyDependencies);

            ((IFightLobbyDeathmatch)lobbyDependencies.Deathmatch).InitDamageHandler(((FightLobbyDependencies)lobbyDependencies).DamageHandler!);
        }
    }
}