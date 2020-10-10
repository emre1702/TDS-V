using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Core.Damagesystem;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Data.Interfaces.LobbySystem.Spectator;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Deathmatch;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.Spectator;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Server.LobbySystem.Weapons;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies.Abstracts
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

        public FightLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            ILoggingHandler loggingHandler, IServiceProvider serviceProvider, ITeamsProvider teamsProvider)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(entity, databaseHandler, langHelper, eventsHandler, loggingHandler, serviceProvider, teamsProvider)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var weaponDatasLoadingHandler = ServiceProvider.GetRequiredService<WeaponDatasLoadingHandler>();
            var damageSys = new Damagesys(LoggingHandler, weaponDatasLoadingHandler);

            lobbyDependencies ??= new FightLobbyDependencies();

            lobbyDependencies.Events ??= new FightLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            ((FightLobbyDependencies)lobbyDependencies).Spectator ??= new FightLobbySpectator(this);
            ((FightLobbyDependencies)lobbyDependencies).Weapons ??= new FightLobbyWeapons(this, (IFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Deathmatch ??= new FightLobbyDeathmatch(this, (IFightLobbyEventsHandler)lobbyDependencies.Events, damageSys, LangHelper);
            lobbyDependencies.Players ??= new FightLobbyPlayers(this, (IFightLobbyEventsHandler)lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new FightLobbyTeamsHandler(this, (IFightLobbyEventsHandler)lobbyDependencies.Events, LangHelper, TeamsProvider);

            Spectator = ((FightLobbyDependencies)lobbyDependencies).Spectator!;
            Weapons = ((FightLobbyDependencies)lobbyDependencies).Weapons!;

            base.InitDependencies(lobbyDependencies);
        }
    }
}
