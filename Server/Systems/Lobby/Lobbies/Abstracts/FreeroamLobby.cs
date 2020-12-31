using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.LobbySystem.Freeroam;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.Freeroam;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies.Abstracts
{
    public abstract class FreeroamLobby : BaseLobby, IFreeroamLobby
    {
        public IFreeroamLobbyFreeroam Freeroam { get; private set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected FreeroamLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler, lobbiesHandler, remoteBrowserEventsHandler)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            var freeroamDataHandler = ServiceProvider.GetRequiredService<FreeroamDataHandler>();

            lobbyDependencies ??= new FreeroamLobbyDependencies();

            ((FreeroamLobbyDependencies)lobbyDependencies).Freeroam ??= new FreeroamLobbyFreeroam(this, freeroamDataHandler);

            Freeroam = ((FreeroamLobbyDependencies)lobbyDependencies).Freeroam!;

            base.InitDependencies(lobbyDependencies);
        }
    }
}